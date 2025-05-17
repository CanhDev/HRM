import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { BehaviorSubject, combineLatest, map, Observable, Subject, Subscription, take } from 'rxjs';
import { finalize, shareReplay, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ApiResponseBasic, PagedResult, SearchRequest } from 'src/app/core/models/base';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';
import { MatDialog } from '@angular/material/dialog';

import { LeaveType } from '../../entity';
import { LeaveTypeService } from '../../leaveType.service';
import { LeaveTypeFilterComponent } from '../leaveTypeFilter/leaveTypeFilter.component';
import { LeaveTypeModalComponent } from '../leaveTypeModal/leaveTypeModal.component';

export interface LeaveTypeWithNames extends LeaveType{
  statusname: string;
  isPaidName: string;
}

@Component({
  selector: 'app-leaveTypeList',
  templateUrl: './leaveTypeList.component.html',
  styleUrls: ['./leaveTypeList.component.css'],
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
export class LeaveTypeListComponent implements OnInit, OnDestroy {


  @ViewChild(LeaveTypeFilterComponent) filterComponent!: LeaveTypeFilterComponent;

  // Core data streams
    searchRequest$: Observable<SearchRequest<LeaveType>>;
    listResponseSubject = new BehaviorSubject<PagedResult<LeaveType> | null>(null);
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
  private LeaveTypesSubject = new BehaviorSubject<LeaveTypeWithNames[]>([]);
  LeaveTypeWithName$ = this.LeaveTypesSubject.asObservable();
  selectedLeaveTypes: number[] = [];

  searchRequest: SearchRequest<LeaveType> = {
    globalSearch: '',
    columnFilters: {
      status: '1'
    },
    dateFilters: {},
    sortBy: 'LeaveTypeCode',
    sortOrder: 'asc',
    page: 1,
    pageSize: 10
  };

  constructor(private router: Router,
    private lookupService: LookupService, private LeaveTypeService: LeaveTypeService, private dialog: MatDialog) { }
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

  this.LeaveTypeService.getData(updatedRequest)
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: res => {
        console.log('test LeaveType: ', res);
        const data = res.data;
        const LeaveTypes = data.items || [];

        const totalPages = Math.ceil(data.totalCount / data.pageSize);

        const listResponse = {
          ...data,
          totalPages
        };

        this.listResponseSubject.next(listResponse);

        this.mapEmployeesWithNames(LeaveTypes);
      },
      error: err => {
        this.errorSubject.next(err);
        this.LeaveTypesSubject.next([]);
        this.listResponseSubject.next(null);
        this.loadingSubject.next(false);
      }
    });
}


  private mapEmployeesWithNames(LeaveTypes: LeaveType[]): void {
  combineLatest([
    this.statusList$
  ])
  .pipe(
    take(1),
    map(([statusList]) => {
      return LeaveTypes.map(emp => {
        const status = statusList.find(s => +s.form_value === +emp.status);

        return {
          ...emp,
          statusname: status?.form_name ?? '---',
          isPaidName: emp.isPaid === 1 ? 'Có' : 'Không'
        } as LeaveTypeWithNames;
      });
    }),
    finalize(() => this.loadingSubject.next(false))
  )
  .subscribe({
    next: mappedLeaveTypes => this.LeaveTypesSubject.next(mappedLeaveTypes),
    error: err => {
      this.errorSubject.next(err);
      this.LeaveTypesSubject.next([]);
    }
  });
}

  refresh(): void {
    this.loadData();
  }

  toggleSelect(id: number): void {
    const index = this.selectedLeaveTypes.indexOf(id);
    if (index === -1) {
      this.selectedLeaveTypes.push(id);
    } else {
      this.selectedLeaveTypes.splice(index, 1);
    }
  }
  isSelected(id: number): boolean {
    return this.selectedLeaveTypes.includes(id);
  }

  toggleSelectAll(): void {
    this.LeaveTypeWithName$.pipe(take(1)).subscribe(LeaveTypes => {
      // If all are selected, unselect all
      if (this.isAllSelected()) {
        this.selectedLeaveTypes = [];
      } else {
        // Otherwise select all
        this.selectedLeaveTypes = LeaveTypes.map(emp => emp.id);
      }
    });
  }

  isAllSelected(): boolean {
    let allSelected = false;
    this.LeaveTypeWithName$.pipe(take(1)).subscribe(LeaveTypes => {
      if (LeaveTypes.length > 0) {
        allSelected = this.selectedLeaveTypes.length === LeaveTypes.length;
      }
    });
    return allSelected;
  }
  editSelected(): void {
      if (this.selectedLeaveTypes.length === 0) {
        alert('Vui lòng chọn ít nhất một loại nghỉ phép để sửa.');
        return;
      }
      
      if (this.selectedLeaveTypes.length > 1) {
        alert('Vui lòng chỉ chọn một loại nghỉ phép để sửa.');
        return;
      }
      
      this.editLeaveType(this.selectedLeaveTypes[0]);
    }
    
    checkStatus(): boolean {
      const LeaveTypes = this.LeaveTypesSubject.getValue();
      return !this.selectedLeaveTypes.some(id => {
        const dept = LeaveTypes.find(d => d.id === id);
        return dept?.status === -1;
      });
    }

    deleteSelected(): void {
      if(!this.checkStatus()){
        return;
      }
      if (this.selectedLeaveTypes.length === 0) {
        alert('Vui lòng chọn ít nhất một loại nghỉ phép để xóa.');
        return;
      }
      
      if (confirm(`Bạn có chắc chắn muốn xóa ${this.selectedLeaveTypes.length} loại nghỉ phép đã chọn?`)) {
       
        if(this.selectedLeaveTypes.length === 1){
          this.LeaveTypeService.delete(this.selectedLeaveTypes[0]).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        else{
          console.log(this.selectedLeaveTypes);
          this.LeaveTypeService.deleteRange(this.selectedLeaveTypes).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        this.selectedLeaveTypes = []; 
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
  deleteLeaveType(id: number, event: Event): void {
      event.stopPropagation();
      if (confirm('Bạn có chắc chắn muốn xóa loại nghỉ phép này?')) {
        this.LeaveTypeService.delete(id).subscribe(res => {
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
  // Thêm loại nghỉ phép
addLeaveType(): void {
  const dialogRef = this.dialog.open(LeaveTypeModalComponent, {
    width: '500px',
    data: {
      LeaveType: {} as LeaveType,
      isEdit: false,
      statusList : this.statusList
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.LeaveTypeService.create(result).subscribe(() => {
        this.refresh(); // Reload lại danh sách
      });
    }
  });
}

// Sửa loại nghỉ phép
editLeaveType(id: number, event?: Event): void {
  if (event) event.stopPropagation();
  const LeaveTypes = this.LeaveTypesSubject.getValue();
  const LeaveType = LeaveTypes.find(d => d.id === id);
  console.log(LeaveType);
  if (!LeaveType) return;

  const dialogRef = this.dialog.open(LeaveTypeModalComponent, {
    width: '500px',
    data: {
      LeaveType: { ...LeaveType },
      isEdit: true,
      statusList : this.statusList
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.LeaveTypeService.update(id, result).subscribe(() => {
        this.refresh();
      });
    }
  });
}
}