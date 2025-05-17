import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { BehaviorSubject, combineLatest, forkJoin, map, Observable, Subject, Subscription, take } from 'rxjs';
import { finalize, shareReplay, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ApiResponseBasic, PagedResult, SearchRequest } from 'src/app/core/models/base';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';
import { MatDialog } from '@angular/material/dialog';

import { ContractAddendum, ContractAddendumDTO, ContractHistory,
   ContractHistoryDTO, EmploymentContract, EmploymentContractDTO, ContractDataset, ContractDatares, RejectModel } from '../../entity';
import { ContractsService } from '../../contracts.service';
import { ContractFilterComponent } from '../contract-filter/contract-filter.component';

export interface contractWithName extends EmploymentContract{
  departmentName: string;
  positionName: string;
  employeeName: string;
  contractTypeName: string;
  statusname: string;
}

@Component({
  selector: 'app-contract-list',
  templateUrl: './contract-list.component.html',
  styleUrls: ['./contract-list.component.css'],
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
export class ContractListComponent implements OnInit, OnDestroy {


  @ViewChild(ContractFilterComponent) filterComponent!: ContractFilterComponent;

  // Core data streams
    searchRequest$: Observable<SearchRequest<EmploymentContract>>;
    listResponseSubject = new BehaviorSubject<PagedResult<EmploymentContract> | null>(null);
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
  departmentList: any[] = [];
  positionList: any[] = [];
  contractTypeList: any[] = [];
  employeeList: any[] = [];

  //
  private contractsSubject = new BehaviorSubject<contractWithName[]>([]);
  contractWithName$ = this.contractsSubject.asObservable();
  selectedcontracts: number[] = [];

  searchRequest: SearchRequest<EmploymentContract> = {
    globalSearch: '',
    columnFilters: {
      status: '2'
    },
    dateFilters: {},
    sortBy: 'contractCode',
    sortOrder: 'asc',
    page: 1,
    pageSize: 10
  };

  constructor(private router: Router,
    private lookupService: LookupService, private contractService: ContractsService) { }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnInit(): void {
  this.loadLookupData();
}

private loadLookupData(): void {
  forkJoin({
    statusList: this.lookupService.getSysDmtt("CONTRACT"),
    departmentList: this.lookupService.getDepartments(),
    positionList: this.lookupService.getPositions(),
    contractTypeList: this.lookupService.getContractTypes(),
    employeeList: this.lookupService.getEmployees()
  })
  .pipe(takeUntil(this.destroy$))
  .subscribe({
    next: result => {
      this.statusList = result.statusList;
      this.departmentList = result.departmentList;
      this.positionList = result.positionList;
      this.contractTypeList = result.contractTypeList;
      this.employeeList = result.employeeList;

      console.log(this.employeeList);

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

  this.contractService.getData(updatedRequest)
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: res => {
        const data = res.data;
        const contracts = data.items || [];
        const totalPages = Math.ceil(data.totalCount / data.pageSize);

        const listResponse = {
          ...data,
          totalPages
        };
        this.listResponseSubject.next(listResponse);

        this.mapContractsWithExtraNames(contracts);
      },
      error: err => {
        this.errorSubject.next(err);
        this.contractsSubject.next([]);
        this.listResponseSubject.next(null);
        this.loadingSubject.next(false);
      }
    });
}

private mapContractsWithExtraNames(contracts: EmploymentContract[]): void {
  const mappedContracts: contractWithName[] = contracts.map(contract => {
    const status = this.statusList.find(s => +s.form_value === +contract.status);
    const department = this.departmentList.find(d => d.form_value === contract.departmentId);
    const position = this.positionList.find(p => p.form_value === contract.positionId);
    const employee = this.employeeList.find(e => e.id == contract.employeeId);
    const contractType = this.contractTypeList.find(c => c.form_value === contract.contractTypeId);

    console.log(employee);

    return {
      ...contract,
      statusname: status?.form_name ?? '---',
      departmentName: department?.form_name ?? '---',
      positionName: position?.form_name ?? '---',
      employeeName: employee?.form_value ?? '---',
      contractTypeName: contractType?.form_name ?? '---'
    };
  });

  this.contractsSubject.next(mappedContracts);
  this.loadingSubject.next(false);
}


  refresh(): void {
    this.filterComponent.resetFilters();
    this.loadData();
  }

  toggleSelect(id: number): void {
    const index = this.selectedcontracts.indexOf(id);
    if (index === -1) {
      this.selectedcontracts.push(id);
    } else {
      this.selectedcontracts.splice(index, 1);
    }
  }
  isSelected(id: number): boolean {
    return this.selectedcontracts.includes(id);
  }

  toggleSelectAll(): void {
    this.contractWithName$.pipe(take(1)).subscribe(contracts => {
      // If all are selected, unselect all
      if (this.isAllSelected()) {
        this.selectedcontracts = [];
      } else {
        // Otherwise select all
        this.selectedcontracts = contracts.map(emp => emp.id);
      }
    });
  }

  isAllSelected(): boolean {
    let allSelected = false;
    this.contractWithName$.pipe(take(1)).subscribe(contracts => {
      if (contracts.length > 0) {
        allSelected = this.selectedcontracts.length === contracts.length;
      }
    });
    return allSelected;
  }
  editSelected(): void {
      if (this.selectedcontracts.length === 0) {
        alert('Vui lòng chọn ít nhất một hợp đồng để sửa.');
        return;
      }
      
      if (this.selectedcontracts.length > 1) {
        alert('Vui lòng chỉ chọn một hợp đồng để sửa.');
        return;
      }
      this.router.navigate(['/employee-contracts/', this.selectedcontracts[0]]);
    }
    viewDetail(id:number):void{
        this.router.navigate(['/employee-contracts/', id]);
    }
    checkStatus(): boolean {
      const contracts = this.contractsSubject.getValue();
      return !this.selectedcontracts.some(id => {
        const dept = contracts.find(d => d.id === id);
        return dept?.status === -1;
      });
    }

    deleteSelected(): void {
      if(!this.checkStatus()){
        return;
      }
      if (this.selectedcontracts.length === 0) {
        alert('Vui lòng chọn ít nhất một hợp đồng để xóa.');
        return;
      }
      
      if (confirm(`Bạn có chắc chắn muốn xóa ${this.selectedcontracts.length} hợp đồng đã chọn?`)) {
       
        if(this.selectedcontracts.length === 1){
          this.contractService.delete(this.selectedcontracts[0]).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        else{
          console.log(this.selectedcontracts);
          this.contractService.deleteRange(this.selectedcontracts).subscribe(res => {
            if(res){
              this.loadData();
            }
          });
        }
        this.selectedcontracts = []; 
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
  deletecontract(id: number, event: Event): void {
      event.stopPropagation();
      if (confirm('Bạn có chắc chắn muốn xóa hợp đồng này?')) {
        this.contractService.delete(id).subscribe(res => {
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
  // Thêm hợp đồng
addcontract(): void {
  this.router.navigate(['/employee-contracts/create']);
}

// Sửa hợp đồng
editcontract(id: number, event: Event): void {
    event.stopPropagation();
    this.router.navigate(['/employee-contracts/', id]);
}
}
