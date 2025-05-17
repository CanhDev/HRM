import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { BehaviorSubject, combineLatest, map, Observable, Subject, Subscription, take } from 'rxjs';
import { finalize, shareReplay, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ApiResponseBasic, PagedResult, SearchRequest } from 'src/app/core/models/base';

import { Position } from '../../entity';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';
import { PositionServiceService } from '../../positionService.service';
import { PositionFilterComponent } from '../position-filter/position-filter.component';
import { PositionsModalComponent } from '../positions-modal/positions-modal.component';
import { MatDialog } from '@angular/material/dialog';

export interface PositionWithNames extends Position{
  statusname:string;
}

@Component({
  selector: 'app-positions-list',
  templateUrl: './positions-list.component.html',
  styleUrls: ['./positions-list.component.css'],
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



export class PositionsListComponent implements OnInit, OnDestroy {


  @ViewChild(PositionFilterComponent) filterComponent!: PositionFilterComponent;

  // Core data streams
    searchRequest$: Observable<SearchRequest<Position>>;
    listResponseSubject = new BehaviorSubject<PagedResult<Position> | null>(null);
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
  private PositionsSubject = new BehaviorSubject<PositionWithNames[]>([]);
  PositionWithName$ = this.PositionsSubject.asObservable();
  selectedPositions: number[] = [];

  searchRequest: SearchRequest<Position> = {
    globalSearch: '',
    columnFilters: {
      status: '1'
    },
    dateFilters: {},
    sortBy: 'positionCode',
    sortOrder: 'asc',
    page: 1,
    pageSize: 10
  };

  constructor(private router: Router,
    private lookupService: LookupService, private PositionService: PositionServiceService, private dialog: MatDialog) { }
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

  this.PositionService.getData(updatedRequest)
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: res => {
        console.log('test Position: ', res);
        const data = res.data;
        const Positions = data.items || [];

        const totalPages = Math.ceil(data.totalCount / data.pageSize);

        const listResponse = {
          ...data,
          totalPages
        };

        this.listResponseSubject.next(listResponse);

        this.mapEmployeesWithNames(Positions);
      },
      error: err => {
        this.errorSubject.next(err);
        this.PositionsSubject.next([]);
        this.listResponseSubject.next(null);
        this.loadingSubject.next(false);
      }
    });
}


  private mapEmployeesWithNames(Positions: Position[]): void {
    combineLatest([
      this.statusList$
    ])
    .pipe(
      take(1),
      map(([statusList]) => {
        return Positions.map(emp => {
          const status = statusList.find(s => +s.form_value === +emp.status);

          return {
            ...emp,
            statusname: status?.form_name ?? '---'
          } as  PositionWithNames;
        });
      }),
      finalize(() => this.loadingSubject.next(false))
    )
    .subscribe({
      next: mappedPositions => this.PositionsSubject.next(mappedPositions),
      error: err => {
        this.errorSubject.next(err);
        this.PositionsSubject.next([]);
      }
    });
  }
  refresh(): void {
    this.loadData();
  }

  toggleSelect(id: number): void {
    const index = this.selectedPositions.indexOf(id);
    if (index === -1) {
      this.selectedPositions.push(id);
    } else {
      this.selectedPositions.splice(index, 1);
    }
  }
  isSelected(id: number): boolean {
    return this.selectedPositions.includes(id);
  }

  toggleSelectAll(): void {
    this.PositionWithName$.pipe(take(1)).subscribe(Positions => {
      // If all are selected, unselect all
      if (this.isAllSelected()) {
        this.selectedPositions = [];
      } else {
        // Otherwise select all
        this.selectedPositions = Positions.map(emp => emp.id);
      }
    });
  }

  isAllSelected(): boolean {
    let allSelected = false;
    this.PositionWithName$.pipe(take(1)).subscribe(Positions => {
      if (Positions.length > 0) {
        allSelected = this.selectedPositions.length === Positions.length;
      }
    });
    return allSelected;
  }
  editSelected(): void {
      if (this.selectedPositions.length === 0) {
        alert('Vui lòng chọn ít nhất một chức vụ để sửa.');
        return;
      }
      
      if (this.selectedPositions.length > 1) {
        alert('Vui lòng chỉ chọn một chức vụ để sửa.');
        return;
      }
      
      this.editPosition(this.selectedPositions[0]);
    }
    
    checkStatus(): boolean {
      const Positions = this.PositionsSubject.getValue();
      return !this.selectedPositions.some(id => {
        const dept = Positions.find(d => d.id === id);
        return dept?.status === -1;
      });
    }

    deleteSelected(): void {
      if(!this.checkStatus()){
        return;
      }
      if (this.selectedPositions.length === 0) {
        alert('Vui lòng chọn ít nhất một chức vụ để xóa.');
        return;
      }
      
      if (confirm(`Bạn có chắc chắn muốn xóa ${this.selectedPositions.length} chức vụ đã chọn?`)) {
       
        if(this.selectedPositions.length === 1){
          this.PositionService.delete(this.selectedPositions[0]).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        else{
          console.log(this.selectedPositions);
          this.PositionService.deleteRange(this.selectedPositions).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        this.selectedPositions = []; 
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
  deletePosition(id: number, event: Event): void {
      event.stopPropagation();
      if (confirm('Bạn có chắc chắn muốn xóa chức vụ này?')) {
        this.PositionService.delete(id).subscribe(res => {
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
  // Thêm chức vụ
addPosition(): void {
  const dialogRef = this.dialog.open(PositionsModalComponent, {
    width: '500px',
    data: {
      Position: {} as Position,
      isEdit: false,
      statusList : this.statusList
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.PositionService.create(result).subscribe(() => {
        this.refresh(); // Reload lại danh sách
      });
    }
  });
}

// Sửa chức vụ
editPosition(id: number, event?: Event): void {
  if (event) event.stopPropagation();
  const Positions = this.PositionsSubject.getValue();
  const Position = Positions.find(d => d.id === id);
  if (!Position) return;

  const dialogRef = this.dialog.open(PositionsModalComponent, {
    width: '500px',
    data: {
      position: { ...Position },
      isEdit: true,
      statusList : this.statusList
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.PositionService.update(id, result).subscribe(() => {
        this.refresh();
      });
    }
  });
}
}
