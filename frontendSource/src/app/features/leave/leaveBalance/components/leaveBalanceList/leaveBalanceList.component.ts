import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { BehaviorSubject, combineLatest, map, Observable, Subject, Subscription, take } from 'rxjs';
import { finalize, shareReplay, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ApiResponseBasic, PagedResult, SearchRequest } from 'src/app/core/models/base';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';
import { MatDialog } from '@angular/material/dialog';

import { EmployeeLeaveBalance } from '../../entity';
import { LeaveBalenceService } from '../../leaveBalence.service';
import { LeaveBalanceFilterComponent } from '../leaveBalanceFilter/leaveBalanceFilter.component';
import { LeaveBalanceModalComponent } from '../leaveBalanceModal/leaveBalanceModal.component';

export interface LeaveBalanceWithNames extends EmployeeLeaveBalance {
  employeeName: string;
  statusname: string;
}

@Component({
  selector: 'app-leaveBalanceList',
  templateUrl: './leaveBalanceList.component.html',
  styleUrls: ['./leaveBalanceList.component.css'],
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
        animate('300ms cubic-bezier(0.4, 0, 0.2, 1)')
      ])
    ])
  ]
})
export class LeaveBalanceListComponent implements OnInit, OnDestroy {


  @ViewChild(LeaveBalanceFilterComponent) filterComponent!: LeaveBalanceFilterComponent;

  // Core data streams
    searchRequest$: Observable<SearchRequest<EmployeeLeaveBalance>>;
    listResponseSubject = new BehaviorSubject<PagedResult<EmployeeLeaveBalance> | null>(null);
    listResponse$ = this.listResponseSubject.asObservable();
    
    // UI state
    showAdvancedFilters = false;
    showFilterContent = false;

    // Data management
  private destroy$ = new Subject<void>();
  private loadingSubject = new BehaviorSubject<boolean>(false);
  loading$ = this.loadingSubject.asObservable();
  private errorSubject = new BehaviorSubject<any>(null);
  error$ = this.errorSubject.asObservable();
  //lookup data
  statusList$ = this.lookupService.getSysListOptions("system", "status").pipe(shareReplay(1));
  statusList: any[] = [];
  employeeList$ = this.lookupService.getEmployees().pipe(shareReplay(1));;
  employeeList: any[] = [];
  //
  private EmployeeLeaveBalancesSubject = new BehaviorSubject<LeaveBalanceWithNames[]>([]);
  EmployeeLeaveBalanceWithName$ = this.EmployeeLeaveBalancesSubject.asObservable();
  selectedEmployeeLeaveBalances: number[] = [];

  searchRequest: SearchRequest<EmployeeLeaveBalance> = {
    globalSearch: '',
    columnFilters: {
      status: '1'
    },
    dateFilters: {},
    sortBy: 'EmployeeLeaveBalanceCode',
    sortOrder: 'asc',
    page: 1,
    pageSize: 10
  };

  constructor(private router: Router,
    private lookupService: LookupService, private LeaveBalenceService: LeaveBalenceService, private dialog: MatDialog) { }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.statusList$.subscribe(data => {
      this.statusList = data;
    });
    this.employeeList$.subscribe(data => this.employeeList = data);
    if (this.filterComponent) {
      this.filterComponent.resetFilters();
    }

    this.loadData();
  }
  loadData(): void {
  const updatedRequest = {
    ...this.searchRequest,
    columnFilters: {
      ...this.searchRequest.columnFilters,
    }
  };

  this.loadingSubject.next(true);

  this.LeaveBalenceService.getData(updatedRequest)
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: res => {
        console.log('test EmployeeLeaveBalance: ', res);
        const data = res.data;
        const EmployeeLeaveBalances = data.items || [];

        const totalPages = Math.ceil(data.totalCount / data.pageSize);

        const listResponse = {
          ...data,
          totalPages
        };

        this.listResponseSubject.next(listResponse);

        this.mapEmployeesWithNames(EmployeeLeaveBalances);
      },
      error: err => {
        this.errorSubject.next(err);
        this.EmployeeLeaveBalancesSubject.next([]);
        this.listResponseSubject.next(null);
        this.loadingSubject.next(false);
      }
    });
}


  private mapEmployeesWithNames(EmployeeLeaveBalances: EmployeeLeaveBalance[]): void {
    combineLatest([
      this.statusList$,
      this.employeeList$
    ])
    .pipe(
      take(1),
      map(([statusList, employeeList]) => {
        return EmployeeLeaveBalances.map(emp => {
          const status = statusList.find(s => +s.form_value === +emp.status);
          const employee = employeeList.find(s => +s.id == +emp.employeeId);

          return {
            ...emp,
            employeeName: employee ? employee.form_value : '',
            statusname: status?.form_name ?? '---'
          } as  LeaveBalanceWithNames;
        });
      }),
      finalize(() => this.loadingSubject.next(false))
    )
    .subscribe({
      next: mappedEmployeeLeaveBalances => this.EmployeeLeaveBalancesSubject.next(mappedEmployeeLeaveBalances),
      error: err => {
        this.errorSubject.next(err);
        this.EmployeeLeaveBalancesSubject.next([]);
      }
    });
  }
  refresh(): void {
    this.loadData();
  }

  toggleSelect(id: number): void {
    const index = this.selectedEmployeeLeaveBalances.indexOf(id);
    if (index === -1) {
      this.selectedEmployeeLeaveBalances.push(id);
    } else {
      this.selectedEmployeeLeaveBalances.splice(index, 1);
    }
  }
  isSelected(id: number): boolean {
    return this.selectedEmployeeLeaveBalances.includes(id);
  }

  toggleSelectAll(): void {
    this.EmployeeLeaveBalanceWithName$.pipe(take(1)).subscribe(EmployeeLeaveBalances => {
      // If all are selected, unselect all
      if (this.isAllSelected()) {
        this.selectedEmployeeLeaveBalances = [];
      } else {
        // Otherwise select all
        this.selectedEmployeeLeaveBalances = EmployeeLeaveBalances.map(emp => emp.id);
      }
    });
  }

  isAllSelected(): boolean {
    let allSelected = false;
    this.EmployeeLeaveBalanceWithName$.pipe(take(1)).subscribe(EmployeeLeaveBalances => {
      if (EmployeeLeaveBalances.length > 0) {
        allSelected = this.selectedEmployeeLeaveBalances.length === EmployeeLeaveBalances.length;
      }
    });
    return allSelected;
  }
  editSelected(): void {
      if (this.selectedEmployeeLeaveBalances.length === 0) {
        alert('Vui lòng chọn ít nhất một loại hợp đồng để sửa.');
        return;
      }
      
      if (this.selectedEmployeeLeaveBalances.length > 1) {
        alert('Vui lòng chỉ chọn một loại hợp đồng để sửa.');
        return;
      }
      
      this.editEmployeeLeaveBalance(this.selectedEmployeeLeaveBalances[0]);
    }
    
    checkStatus(): boolean {
      const EmployeeLeaveBalances = this.EmployeeLeaveBalancesSubject.getValue();
      return !this.selectedEmployeeLeaveBalances.some(id => {
        const dept = EmployeeLeaveBalances.find(d => d.id === id);
        return dept?.status === -1;
      });
    }

    
    onSearch(globalSearch: string): void {
    this.searchRequest.globalSearch = globalSearch;
    this.searchRequest.page = 1;
    this.loadData();
    }

  onFilterChange(filters: any): void {
    this.searchRequest.columnFilters = { ...filters.columnFilters };
    this.searchRequest.dateFilters = { ...filters.dateFilters };
    this.searchRequest.page = 1;
    this.loadData();
  }

  onSort(sortBy: string): void {
    const isSameSort = this.searchRequest.sortBy === sortBy;
    this.searchRequest.sortBy = sortBy;
    this.searchRequest.sortOrder = isSameSort && this.searchRequest.sortOrder === 'asc' ? 'desc' : 'asc';
    this.loadData();
  }

  onPageChange(page: number): void {
    this.searchRequest.page = page;
    this.loadData();
  }

  onPageSizeChange(pageSize: number): void {
    this.searchRequest.pageSize = pageSize;
    this.searchRequest.page = 1;
    this.loadData();
  }
//handlers
  

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

  // modal


// Sửa loại hợp đồng
editEmployeeLeaveBalance(id: number, event?: Event): void {
  if (event) event.stopPropagation();
  const EmployeeLeaveBalances = this.EmployeeLeaveBalancesSubject.getValue();
  const EmployeeLeaveBalance = EmployeeLeaveBalances.find(d => d.id === id);
  if (!EmployeeLeaveBalance) return;

  const dialogRef = this.dialog.open(LeaveBalanceModalComponent, {
    width: '500px',
    data: {
      EmployeeLeaveBalance: { ...EmployeeLeaveBalance },
      isEdit: true,
      statusList : this.statusList
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.LeaveBalenceService.update(id, result).subscribe(() => {
        this.refresh();
      });
    }
  });
}
}
