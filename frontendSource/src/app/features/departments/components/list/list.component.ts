import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { BehaviorSubject, combineLatest, map, Observable, Subject, Subscription, take } from 'rxjs';
import { finalize, shareReplay, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ApiResponseBasic, PagedResult, SearchRequest } from 'src/app/core/models/base';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';
import { MatDialog } from '@angular/material/dialog';

import { Department } from '../../entity';
import { FilterComponent } from '../filter/filter.component';
import { DepartmentService } from '../../department.service';
import { DepartmentModalComponent } from '../department-modal/department-modal.component';


export interface DepartmentWithName extends Department{
  statusname: string;
}

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css'],
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
export class ListComponent implements OnInit, OnDestroy {


  @ViewChild(FilterComponent) filterComponent!: FilterComponent;

  // Core data streams
    searchRequest$: Observable<SearchRequest<Department>>;
    listResponseSubject = new BehaviorSubject<PagedResult<Department> | null>(null);
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

  //
  private departmentsSubject = new BehaviorSubject<DepartmentWithName[]>([]);
  DepartmentWithName$ = this.departmentsSubject.asObservable();
  selecteddepartments: number[] = [];

  searchRequest: SearchRequest<Department> = {
    globalSearch: '',
    columnFilters: {
      status: '1'
    },
    dateFilters: {},
    sortBy: 'departmentCode',
    sortOrder: 'asc',
    page: 1,
    pageSize: 10
  };

  constructor(private router: Router,
    private lookupService: LookupService, private departmentService: DepartmentService, private dialog: MatDialog) { }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnInit(): void {
    this.statusList$.subscribe(data => {
      this.statusList = data;
    });
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

  this.departmentService.getData(updatedRequest)
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: res => {
        console.log('test department: ', res);
        const data = res.data;
        const departments = data.items || [];

        const totalPages = Math.ceil(data.totalCount / data.pageSize);

        const listResponse = {
          ...data,
          totalPages
        };

        this.listResponseSubject.next(listResponse);

        this.mapEmployeesWithNames(departments);
      },
      error: err => {
        this.errorSubject.next(err);
        this.departmentsSubject.next([]);
        this.listResponseSubject.next(null);
        this.loadingSubject.next(false);
      }
    });
}


  private mapEmployeesWithNames(departments: Department[]): void {
    combineLatest([
      this.statusList$
    ])
    .pipe(
      take(1),
      map(([statusList]) => {
        return departments.map(emp => {
          const status = statusList.find(s => +s.form_value === +emp.status);

          return {
            ...emp,
            statusname: status?.form_name ?? '---'
          } as  DepartmentWithName;
        });
      }),
      finalize(() => this.loadingSubject.next(false))
    )
    .subscribe({
      next: mappedDepartments => this.departmentsSubject.next(mappedDepartments),
      error: err => {
        this.errorSubject.next(err);
        this.departmentsSubject.next([]);
      }
    });
  }
  refresh(): void {
    this.loadData();
  }

  toggleSelect(id: number): void {
    const index = this.selecteddepartments.indexOf(id);
    if (index === -1) {
      this.selecteddepartments.push(id);
    } else {
      this.selecteddepartments.splice(index, 1);
    }
  }
  isSelected(id: number): boolean {
    return this.selecteddepartments.includes(id);
  }

  toggleSelectAll(): void {
    this.DepartmentWithName$.pipe(take(1)).subscribe(departments => {
      // If all are selected, unselect all
      if (this.isAllSelected()) {
        this.selecteddepartments = [];
      } else {
        // Otherwise select all
        this.selecteddepartments = departments.map(emp => emp.id);
      }
    });
  }

  isAllSelected(): boolean {
    let allSelected = false;
    this.DepartmentWithName$.pipe(take(1)).subscribe(departments => {
      if (departments.length > 0) {
        allSelected = this.selecteddepartments.length === departments.length;
      }
    });
    return allSelected;
  }
  editSelected(): void {
      if (this.selecteddepartments.length === 0) {
        alert('Vui lòng chọn ít nhất một phòng ban để sửa.');
        return;
      }
      
      if (this.selecteddepartments.length > 1) {
        alert('Vui lòng chỉ chọn một phòng ban để sửa.');
        return;
      }
      
      this.editDepartment(this.selecteddepartments[0]);
    }
    
    checkStatus(): boolean {
      const departments = this.departmentsSubject.getValue();
      return !this.selecteddepartments.some(id => {
        const dept = departments.find(d => d.id === id);
        return dept?.status === -1;
      });
    }

    deleteSelected(): void {
      if(!this.checkStatus()){
        return;
      }
      if (this.selecteddepartments.length === 0) {
        alert('Vui lòng chọn ít nhất một phòng ban để xóa.');
        return;
      }
      
      if (confirm(`Bạn có chắc chắn muốn xóa ${this.selecteddepartments.length} phòng ban đã chọn?`)) {
       
        if(this.selecteddepartments.length === 1){
          this.departmentService.delete(this.selecteddepartments[0]).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        else{
          console.log(this.selecteddepartments);
          this.departmentService.deleteRange(this.selecteddepartments).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        this.selecteddepartments = []; 
      }
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
  deleteDepartment(id: number, event: Event): void {
      event.stopPropagation();
      if (confirm('Bạn có chắc chắn muốn xóa phòng ban này?')) {
        this.departmentService.delete(id).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
      }
    }

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
  // Thêm phòng ban
addDepartment(): void {
  const dialogRef = this.dialog.open(DepartmentModalComponent, {
    width: '500px',
    data: {
      department: {} as Department,
      isEdit: false,
      statusList : this.statusList
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.departmentService.create(result).subscribe(() => {
        this.refresh(); // Reload lại danh sách
      });
    }
  });
}

// Sửa phòng ban
editDepartment(id: number, event?: Event): void {
  if (event) event.stopPropagation();
  const departments = this.departmentsSubject.getValue();
  const department = departments.find(d => d.id === id);
  if (!department) return;

  const dialogRef = this.dialog.open(DepartmentModalComponent, {
    width: '500px',
    data: {
      department: { ...department },
      isEdit: true,
      statusList : this.statusList
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.departmentService.update(id, result).subscribe(() => {
        this.refresh();
      });
    }
  });
}
}
