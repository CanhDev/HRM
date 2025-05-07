import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { BehaviorSubject, combineLatest, map, Observable, Subject, Subscription, take } from 'rxjs';
import { finalize, shareReplay, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { trigger, state, style, transition, animate } from '@angular/animations';

import { Employee } from '../../../models/entity';
import { ApiResponseBasic, PagedResult, SearchRequest } from 'src/app/core/models/base';
import * as EmployeeDocActions from '../../../store/actions/employee-doc.actions';
import * as EmployeeSelectors from '../../../store/selectors/employee-doc.selectors';
import { EmployeeFilterComponent } from '../employee-filter/employee-filter.component';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';

// Interface for enhanced employee with department and position names
export interface EmployeeWithNames extends Employee {
  departmentName: string;
  positionName: string;
  statusName:string;
}

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css'],
  animations: [
    trigger('expandCollapse', [
      state('collapsed', style({
        height: '0',
        opacity: '0',
        overflow: 'hidden'
      })),
      state('expanded', style({
        height: '*',
        opacity: '1'
      })),
      transition('collapsed <=> expanded', [
        animate('600ms cubic-bezier(0.4, 0, 0.2, 1)')
      ])
    ])
  ]
})
export class EmployeeListComponent implements OnInit, OnDestroy {
  @ViewChild(EmployeeFilterComponent) filterComponent!: EmployeeFilterComponent;

  // Core data streams
  searchRequest$: Observable<SearchRequest<Employee>>;
  listResponse$: Observable<PagedResult<Employee> | null>;
  
  // UI state
  showAdvancedFilters = false;
  showFilterContent = false;
  
  // Data management
  private destroy$ = new Subject<void>();
  private loadingSubject = new BehaviorSubject<boolean>(false);
  loading$ = this.loadingSubject.asObservable();
  
  private errorSubject = new BehaviorSubject<any>(null);
  error$ = this.errorSubject.asObservable();
  
  // Lookup data
  departments$ = this.lookupService.getDepartments().pipe(shareReplay(1));
  positions$ = this.lookupService.getPositions().pipe(shareReplay(1));
  statusList$ = this.lookupService.getSysListOptions("Employee", "status").pipe(shareReplay(1));
  
  // The main data source for display - processed employees with department/position names
  private employeesSubject = new BehaviorSubject<EmployeeWithNames[]>([]);
  employeesWithNames$ = this.employeesSubject.asObservable();
  selectedEmployees: number[] = [];
  constructor(
    private store: Store,
    private router: Router,
    private lookupService: LookupService
  ) {
    // Get references to core state selectors
    this.searchRequest$ = this.store.select(EmployeeSelectors.selectSearchRequest);
    this.listResponse$ = this.store.select(EmployeeSelectors.selectListResponse);
  }

  ngOnInit(): void {
    // Reset filters if filter component is available
    if (this.filterComponent) {
      this.filterComponent.resetFilters();
    }
    
    // Start by setting up combined data streams
    this.setupDataSubscriptions();
    
    // Initial data load
    this.loadData();
  }

  ngOnDestroy(): void {
    // Clean up all subscriptions
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Set up the main data processing pipeline
  private setupDataSubscriptions(): void {
    // Listen for employee data changes and map with lookup data
    this.store.select(EmployeeSelectors.selectEmployees)
      .pipe(takeUntil(this.destroy$))
      .subscribe(employees => {
        if (employees && employees.length > 0) {
          this.mapEmployeesWithNames(employees);
        } else {
          this.employeesSubject.next([]);
        }
      });
      
    // Listen for loading state changes
    this.store.select(EmployeeSelectors.selectLoading)
      .pipe(takeUntil(this.destroy$))
      .subscribe(loading => {
        this.loadingSubject.next(loading);
      });
      
    // Listen for error state changes
    this.store.select(EmployeeSelectors.selectError)
      .pipe(takeUntil(this.destroy$))
      .subscribe(error => {
        this.errorSubject.next(error);
      });
  }

  // Map employees with department and position names
  private mapEmployeesWithNames(employees: Employee[]): void {
    this.loadingSubject.next(true);
    
    // Wait for both lookup datasets to be available
    combineLatest([
      this.departments$,
      this.positions$,
      this.statusList$
    ]).pipe(
      take(1),
      map(([departments, positions, statusList]) => {
        return employees.map(emp => {
          const department = departments.find(d => Number(d.form_value) === Number(emp.departmentId));
          const position = positions.find(p => Number(p.form_value) === Number(emp.positionId));
          const status = statusList.find(p => Number(p.form_value) === Number(emp.status));
          console.log("test",emp);
          return {
            ...emp,
            departmentName: department ? department.form_name : '---',
            positionName: position ? position.form_name : '---',
            statusName: status ? status.form_name : '---'
          } as EmployeeWithNames;
        });
      }),
      finalize(() => this.loadingSubject.next(false))
    ).subscribe({
      next: (mappedEmployees) => {
        this.employeesSubject.next(mappedEmployees);
      },
      error: (err) => {
        this.errorSubject.next(err);
        this.employeesSubject.next([]);
      }
    });
  }

  // Fetch data based on current search request
  loadData(): void {
    this.searchRequest$.pipe(take(1)).subscribe(searchRequest => {
      const updatedRequest = {
        ...searchRequest,
        columnFilters: {
          ...(searchRequest.columnFilters || {}),
          status: '1'
        }
      };
      this.store.dispatch(EmployeeDocActions.loadEmployees({ searchRequest: updatedRequest }));
    });
  }
  
  
  // Refresh data with current search parameters
  refresh(): void {
    this.loadData();
  }

  // Selection methods
  toggleSelect(id: number): void {
    const index = this.selectedEmployees.indexOf(id);
    if (index === -1) {
      this.selectedEmployees.push(id);
    } else {
      this.selectedEmployees.splice(index, 1);
    }
  }

  isSelected(id: number): boolean {
    return this.selectedEmployees.includes(id);
  }

  toggleSelectAll(): void {
    this.employeesWithNames$.pipe(take(1)).subscribe(employees => {
      // If all are selected, unselect all
      if (this.isAllSelected()) {
        this.selectedEmployees = [];
      } else {
        // Otherwise select all
        this.selectedEmployees = employees.map(emp => emp.id);
      }
    });
  }

  isAllSelected(): boolean {
    let allSelected = false;
    this.employeesWithNames$.pipe(take(1)).subscribe(employees => {
      if (employees.length > 0) {
        allSelected = this.selectedEmployees.length === employees.length;
      }
    });
    return allSelected;
  }

  // Action handlers
  editSelected(): void {
    if (this.selectedEmployees.length === 0) {
      alert('Vui lòng chọn ít nhất một nhân viên để sửa.');
      return;
    }
    
    if (this.selectedEmployees.length > 1) {
      alert('Vui lòng chỉ chọn một nhân viên để sửa.');
      return;
    }
    
    // Navigate to edit page for the selected employee
    this.router.navigate(['/employees/', this.selectedEmployees[0]]);
  }

  deleteSelected(): void {
    if (this.selectedEmployees.length === 0) {
      alert('Vui lòng chọn ít nhất một nhân viên để xóa.');
      return;
    }
    
    if (confirm(`Bạn có chắc chắn muốn xóa ${this.selectedEmployees.length} nhân viên đã chọn?`)) {
      // In a real application, we would dispatch a batch delete action here
      // For demonstration purposes, we'll just delete one by one
      this.selectedEmployees.forEach(id => {
        this.store.dispatch(EmployeeDocActions.deleteEmployee({ id }));
      });
      
      this.selectedEmployees = []; // Clear selections after delete
    }
  }
  // Action handlers
  

  // Search & filter handling
  onSearch(globalSearch: string): void {
    this.searchRequest$.pipe(take(1)).subscribe(currentSearchRequest => {
      const updatedSearchRequest = {
        ...currentSearchRequest,
        globalSearch,
        page: 1 // Reset to first page when searching
      };
      this.store.dispatch(EmployeeDocActions.setSearchRequest({ searchRequest: updatedSearchRequest }));
    });
  }

  onFilterChange(filters: any): void {
    this.searchRequest$.pipe(take(1)).subscribe(currentSearchRequest => {
      const updatedSearchRequest = {
        ...currentSearchRequest,
        columnFilters: { ...filters.columnFilters },
        dateFilters: { ...filters.dateFilters },
        page: 1 // Reset to first page when filters change
      };
      this.store.dispatch(EmployeeDocActions.setSearchRequest({ searchRequest: updatedSearchRequest }));
    });
  }

  onSort(sortBy: string): void {
    this.searchRequest$.pipe(take(1)).subscribe((currentSearchRequest: SearchRequest<Employee>) => {
      const sortOrder: 'asc' | 'desc' = 
        currentSearchRequest.sortBy === sortBy && currentSearchRequest.sortOrder === 'asc'
          ? 'desc'
          : 'asc';
  
      const updatedSearchRequest: SearchRequest<Employee> = {
        ...currentSearchRequest,
        sortBy,
        sortOrder 
      };
  
      this.store.dispatch(EmployeeDocActions.setSearchRequest({ searchRequest: updatedSearchRequest }));
    });
  }

  // Pagination handling
  onPageChange(page: number): void {
    this.searchRequest$.pipe(take(1)).subscribe(currentSearchRequest => {
      const updatedSearchRequest = {
        ...currentSearchRequest,
        page
      };
      
      this.store.dispatch(EmployeeDocActions.setSearchRequest({ searchRequest: updatedSearchRequest }));
    });
  }

  onPageSizeChange(pageSize: number): void {
    this.searchRequest$.pipe(take(1)).subscribe(currentSearchRequest => {
      const updatedSearchRequest = {
        ...currentSearchRequest,
        pageSize,
        page: 1 // Reset to first page when page size changes
      };
      
      this.store.dispatch(EmployeeDocActions.setSearchRequest({ searchRequest: updatedSearchRequest }));
    });
  }

  // Navigation handlers
  viewEmployeeDetails(id: number): void {
    this.router.navigate(['/employees', id]);
  }

  createEmployee(): void {
    this.router.navigate(['/employees/create']);
  }

  deleteEmployee(id: number, event: Event): void {
    event.stopPropagation();
    if (confirm('Bạn có chắc chắn muốn xóa nhân viên này?')) {
      this.store.dispatch(EmployeeDocActions.deleteEmployee({ id }));
    }
  }

  editEmployee(id: number, event: Event): void {
    event.stopPropagation();
    this.router.navigate(['/employees/edit', id]);
  }

  // Advanced filters toggle
  toggleAdvancedFilters(): void {
    if (this.showAdvancedFilters) {
      // Start collapse animation, then hide content after animation ends
      this.showAdvancedFilters = false;
    } else {
      // Show content immediately, then expand
      this.showFilterContent = true;
      this.showAdvancedFilters = true;
      this.scrollToFilterSection();
    }
  }

  // Scroll to filter section when expanded
  scrollToFilterSection(): void {
    setTimeout(() => {
      const filterSection = document.querySelector('.filter-section');
      if (filterSection) {
        filterSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    }, 300); // Delay to allow animation to start
  }

  // Animation completion handler
  onAnimationDone(event: any): void {
    if (event.toState === 'collapsed') {
      this.showFilterContent = false;
    }
  }

  // Generate pagination numbers
  getPageNumbers(currentPage: number, totalPages: number): number[] {
    const pages: number[] = [];
    
    if (totalPages <= 7) {
      // If fewer than 7 pages, show all
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
      }
    } else {
      // Always show first page
      pages.push(1);
      
      // Handle case when current page is near the beginning
      if (currentPage <= 3) {
        pages.push(2, 3, 4, 0, totalPages - 1, totalPages);
      } 
      // Handle case when current page is near the end
      else if (currentPage >= totalPages - 2) {
        pages.push(0, totalPages - 4, totalPages - 3, totalPages - 2, totalPages - 1, totalPages);
      } 
      // Handle case when current page is in the middle
      else {
        pages.push(0, currentPage - 1, currentPage, currentPage + 1, 0, totalPages);
      }
    }
    
    return pages;
  }
}