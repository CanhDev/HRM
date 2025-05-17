import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { BehaviorSubject, combineLatest, forkJoin, map, Observable, Subject, Subscription, take } from 'rxjs';
import { finalize, shareReplay, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ApiResponseBasic, PagedResult, SearchRequest } from 'src/app/core/models/base';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';
import { MatDialog } from '@angular/material/dialog';

import { LeaveRequest } from '../../entity';
import { LeaveRequestService } from '../../leaveRequest.service';
import { LeaveRequestFilterComponent } from '../leaveRequestFilter/leaveRequestFilter.component';

export interface leaveRequestWithNames extends LeaveRequest {
  employeeName: string;
  statusname: string;
}

@Component({
  selector: 'app-leaveRequestList',
  templateUrl: './leaveRequestList.component.html',
  styleUrls: ['./leaveRequestList.component.css'],
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
export class LeaveRequestListComponent implements OnInit, OnDestroy {


  @ViewChild(LeaveRequestFilterComponent) filterComponent!: LeaveRequestFilterComponent;

  // Core data streams
    searchRequest$: Observable<SearchRequest<LeaveRequest>>;
    listResponseSubject = new BehaviorSubject<PagedResult<LeaveRequest> | null>(null);
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
  

  statusList: any[] = [];
  employeeList: any[] = [];

  //
  private leaveRequestSubject = new BehaviorSubject<leaveRequestWithNames[]>([]);
  leaveRequestWithName$ = this.leaveRequestSubject.asObservable();
  selectedleaveRequest: number[] = [];

  searchRequest: SearchRequest<LeaveRequest> = {
    globalSearch: '',
    columnFilters: {
    },
    dateFilters: {},
    sortBy: 'createDate',
    sortOrder: 'asc',
    page: 1,
    pageSize: 10
  };

  constructor(private router: Router,
    private lookupService: LookupService, private leaveRequestService: LeaveRequestService) { }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnInit(): void {
  this.loadLookupData();
}

private loadLookupData(): void {
  forkJoin({
    statusList: this.lookupService.getSysDmtt("LEAVE"),
    employeeList: this.lookupService.getEmployees(),
    leaveTypeList : this.lookupService.getLeaveTypes()
  })
  .pipe(takeUntil(this.destroy$))
  .subscribe({
    next: result => {
      this.statusList = result.statusList;
      this.employeeList = result.employeeList;

      if (this.filterComponent) {
        this.filterComponent.resetFilters();
      }

      this.loadData();
    },
    error: err => {
      console.error('Failed to load lookup data', err);
      this.errorSubject.next(err);
    }
  });
}

private loadData(): void {
  const updatedRequest = {
    ...this.searchRequest,
    columnFilters: {
      ...this.searchRequest.columnFilters,
    }
  };

  this.loadingSubject.next(true);

  this.leaveRequestService.getData(updatedRequest)
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: res => {
        const data = res.data;
        const leaveRequest = data.items || [];
        const totalPages = Math.ceil(data.totalCount / data.pageSize);

        const listResponse = {
          ...data,
          totalPages
        };
        this.listResponseSubject.next(listResponse);

        this.mapleaveRequestWithExtraNames(leaveRequest);
      },
      error: err => {
        this.errorSubject.next(err);
        this.leaveRequestSubject.next([]);
        this.listResponseSubject.next(null);
        this.loadingSubject.next(false);
      }
    });
}

private mapleaveRequestWithExtraNames(leaveRequest: LeaveRequest[]): void {
  const mappedleaveRequest: leaveRequestWithNames[] = leaveRequest.map(leaveRequest => {
    const status = this.statusList.find(s => +s.form_value === +leaveRequest.approvalStatus);
    const employee = this.employeeList.find(e => e.id == leaveRequest.employeeId);

    console.log(employee);

    return {
      ...leaveRequest,
      statusname: status?.form_name ?? '---',
      employeeName: employee?.form_value ?? '---',
    };
  });

  this.leaveRequestSubject.next(mappedleaveRequest);
  this.loadingSubject.next(false);
}


  refresh(): void {
    this.filterComponent.resetFilters();
    this.loadData();
  }

  toggleSelect(id: number): void {
    const index = this.selectedleaveRequest.indexOf(id);
    if (index === -1) {
      this.selectedleaveRequest.push(id);
    } else {
      this.selectedleaveRequest.splice(index, 1);
    }
  }
  isSelected(id: number): boolean {
    return this.selectedleaveRequest.includes(id);
  }

  toggleSelectAll(): void {
    this.leaveRequestWithName$.pipe(take(1)).subscribe(leaveRequest => {
      // If all are selected, unselect all
      if (this.isAllSelected()) {
        this.selectedleaveRequest = [];
      } else {
        // Otherwise select all
        this.selectedleaveRequest = leaveRequest.map(emp => emp.id);
      }
    });
  }

  isAllSelected(): boolean {
    let allSelected = false;
    this.leaveRequestWithName$.pipe(take(1)).subscribe(leaveRequest => {
      if (leaveRequest.length > 0) {
        allSelected = this.selectedleaveRequest.length === leaveRequest.length;
      }
    });
    return allSelected;
  }
  editSelected(): void {
      if (this.selectedleaveRequest.length === 0) {
        alert('Vui lòng chọn ít nhất một phiếu để sửa.');
        return;
      }
      
      if (this.selectedleaveRequest.length > 1) {
        alert('Vui lòng chỉ chọn một phiếu để sửa.');
        return;
      }
      this.router.navigate(['/employee-leaveRequest/', this.selectedleaveRequest[0]]);
    }
    viewDetail(id:number):void{
        this.router.navigate(['/employee-leaveRequest/', id]);
    }
    checkStatus(): boolean {
      const leaveRequest = this.leaveRequestSubject.getValue();
      return !this.selectedleaveRequest.some(id => {
        const dept = leaveRequest.find(d => d.id === id);
        return dept?.approvalStatus === -1;
      });
    }

    deleteSelected(): void {
      if(!this.checkStatus()){
        return;
      }
      if (this.selectedleaveRequest.length === 0) {
        alert('Vui lòng chọn ít nhất một phiếu để xóa.');
        return;
      }
      
      if (confirm(`Bạn có chắc chắn muốn xóa ${this.selectedleaveRequest.length} phiếu đã chọn?`)) {
       
        if(this.selectedleaveRequest.length === 1){
          this.leaveRequestService.delete(this.selectedleaveRequest[0]).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        else{
          console.log(this.selectedleaveRequest);
          this.leaveRequestService.deleteRange(this.selectedleaveRequest).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        this.selectedleaveRequest = []; 
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
  deleteleaveRequest(id: number, event: Event): void {
      event.stopPropagation();
      if (confirm('Bạn có chắc chắn muốn xóa phiếu này?')) {
        this.leaveRequestService.delete(id).subscribe(res => {
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
  // Thêm phiếu
addleaveRequest(): void {
  this.router.navigate(['/leave-requests/create']);
}

// Sửa phiếu
editleaveRequest(id: number, event: Event): void {
    event.stopPropagation();
    this.router.navigate(['/leave-requests/', id]);
}
}
