import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { BehaviorSubject, combineLatest, map, Observable, Subject, Subscription, take } from 'rxjs';
import { finalize, shareReplay, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ApiResponseBasic, PagedResult, SearchRequest } from 'src/app/core/models/base';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';
import { MatDialog } from '@angular/material/dialog';

import { ContractType } from '../../entity';
import { ContractTypeService } from '../../contractType.service';
import { ContractTypeFilterComponent } from '../contractType-filter/contractType-filter.component';
import { ContractTypeModalComponent } from '../contractType-modal/contractType-modal.component';

export interface ContractTypeWithNames extends ContractType {
  statusname: string;
}

@Component({
  selector: 'app-contractType-list',
  templateUrl: './contractType-list.component.html',
  styleUrls: ['./contractType-list.component.css'],
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
export class ContractTypeListComponent implements OnInit, OnDestroy {


  @ViewChild(ContractTypeFilterComponent) filterComponent!: ContractTypeFilterComponent;

  // Core data streams
    searchRequest$: Observable<SearchRequest<ContractType>>;
    listResponseSubject = new BehaviorSubject<PagedResult<ContractType> | null>(null);
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
  private ContractTypesSubject = new BehaviorSubject<ContractTypeWithNames[]>([]);
  ContractTypeWithName$ = this.ContractTypesSubject.asObservable();
  selectedContractTypes: number[] = [];

  searchRequest: SearchRequest<ContractType> = {
    globalSearch: '',
    columnFilters: {
      status: '1'
    },
    dateFilters: {},
    sortBy: 'ContractTypeCode',
    sortOrder: 'asc',
    page: 1,
    pageSize: 10
  };

  constructor(private router: Router,
    private lookupService: LookupService, private ContractTypeService: ContractTypeService, private dialog: MatDialog) { }
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

  this.ContractTypeService.getData(updatedRequest)
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: res => {
        console.log('test ContractType: ', res);
        const data = res.data;
        const ContractTypes = data.items || [];

        const totalPages = Math.ceil(data.totalCount / data.pageSize);

        const listResponse = {
          ...data,
          totalPages
        };

        this.listResponseSubject.next(listResponse);

        this.mapEmployeesWithNames(ContractTypes);
      },
      error: err => {
        this.errorSubject.next(err);
        this.ContractTypesSubject.next([]);
        this.listResponseSubject.next(null);
        this.loadingSubject.next(false);
      }
    });
}


  private mapEmployeesWithNames(ContractTypes: ContractType[]): void {
    combineLatest([
      this.statusList$
    ])
    .pipe(
      take(1),
      map(([statusList]) => {
        return ContractTypes.map(emp => {
          const status = statusList.find(s => +s.form_value === +emp.status);

          return {
            ...emp,
            statusname: status?.form_name ?? '---'
          } as  ContractTypeWithNames;
        });
      }),
      finalize(() => this.loadingSubject.next(false))
    )
    .subscribe({
      next: mappedContractTypes => this.ContractTypesSubject.next(mappedContractTypes),
      error: err => {
        this.errorSubject.next(err);
        this.ContractTypesSubject.next([]);
      }
    });
  }
  refresh(): void {
    this.loadData();
  }

  toggleSelect(id: number): void {
    const index = this.selectedContractTypes.indexOf(id);
    if (index === -1) {
      this.selectedContractTypes.push(id);
    } else {
      this.selectedContractTypes.splice(index, 1);
    }
  }
  isSelected(id: number): boolean {
    return this.selectedContractTypes.includes(id);
  }

  toggleSelectAll(): void {
    this.ContractTypeWithName$.pipe(take(1)).subscribe(ContractTypes => {
      // If all are selected, unselect all
      if (this.isAllSelected()) {
        this.selectedContractTypes = [];
      } else {
        // Otherwise select all
        this.selectedContractTypes = ContractTypes.map(emp => emp.id);
      }
    });
  }

  isAllSelected(): boolean {
    let allSelected = false;
    this.ContractTypeWithName$.pipe(take(1)).subscribe(ContractTypes => {
      if (ContractTypes.length > 0) {
        allSelected = this.selectedContractTypes.length === ContractTypes.length;
      }
    });
    return allSelected;
  }
  editSelected(): void {
      if (this.selectedContractTypes.length === 0) {
        alert('Vui lòng chọn ít nhất một loại hợp đồng để sửa.');
        return;
      }
      
      if (this.selectedContractTypes.length > 1) {
        alert('Vui lòng chỉ chọn một loại hợp đồng để sửa.');
        return;
      }
      
      this.editContractType(this.selectedContractTypes[0]);
    }
    
    checkStatus(): boolean {
      const ContractTypes = this.ContractTypesSubject.getValue();
      return !this.selectedContractTypes.some(id => {
        const dept = ContractTypes.find(d => d.id === id);
        return dept?.status === -1;
      });
    }

    deleteSelected(): void {
      if(!this.checkStatus()){
        return;
      }
      if (this.selectedContractTypes.length === 0) {
        alert('Vui lòng chọn ít nhất một loại hợp đồng để xóa.');
        return;
      }
      
      if (confirm(`Bạn có chắc chắn muốn xóa ${this.selectedContractTypes.length} loại hợp đồng đã chọn?`)) {
       
        if(this.selectedContractTypes.length === 1){
          this.ContractTypeService.delete(this.selectedContractTypes[0]).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        else{
          console.log(this.selectedContractTypes);
          this.ContractTypeService.deleteRange(this.selectedContractTypes).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        this.selectedContractTypes = []; 
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
  deleteContractType(id: number, event: Event): void {
      event.stopPropagation();
      if (confirm('Bạn có chắc chắn muốn xóa loại hợp đồng này?')) {
        this.ContractTypeService.delete(id).subscribe(res => {
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
  // Thêm loại hợp đồng
addContractType(): void {
  const dialogRef = this.dialog.open(ContractTypeModalComponent, {
    width: '500px',
    data: {
      ContractType: {} as ContractType,
      isEdit: false,
      statusList : this.statusList
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.ContractTypeService.create(result).subscribe(() => {
        this.refresh(); // Reload lại danh sách
      });
    }
  });
}

// Sửa loại hợp đồng
editContractType(id: number, event?: Event): void {
  if (event) event.stopPropagation();
  const ContractTypes = this.ContractTypesSubject.getValue();
  const ContractType = ContractTypes.find(d => d.id === id);
  if (!ContractType) return;

  const dialogRef = this.dialog.open(ContractTypeModalComponent, {
    width: '500px',
    data: {
      contractType: { ...ContractType },
      isEdit: true,
      statusList : this.statusList
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      this.ContractTypeService.update(id, result).subscribe(() => {
        this.refresh();
      });
    }
  });
}
}
