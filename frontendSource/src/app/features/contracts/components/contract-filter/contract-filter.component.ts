import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { debounceTime, forkJoin, map, Observable, shareReplay, startWith } from 'rxjs';
import { ApiResponseBasic, PagedResult, SearchRequest, DateRange } from 'src/app/core/models/base';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';

@Component({
  selector: 'app-contract-filter',
  templateUrl: './contract-filter.component.html',
  styleUrls: ['./contract-filter.component.css']
})
export class ContractFilterComponent implements OnInit {

 @Output() filterChange = new EventEmitter<{
    columnFilters: { [key: string]: string | null },
    dateFilters: { [key: string]: DateRange }
  }>();
  filterForm!: FormGroup;
  private valueChangesSub: any;
  constructor(private fb: FormBuilder,private lookupService: LookupService) {}
  

  statusList: any[] = [];
  departmentList: any[] = [];
  positionList: any[] = [];
  contractTypeList: any[] = [];
  employeeList: any[] = [];


  ngOnInit() {
  forkJoin({
    statusList: this.lookupService.getSysDmtt("CONTRACT"),
    departmentList: this.lookupService.getDepartments(),
    positionList: this.lookupService.getPositions(),
    contractTypeList: this.lookupService.getContractTypes(),
    employeeList: this.lookupService.getEmployees()
  }).subscribe(result => {
    this.statusList = result.statusList;
    this.departmentList = result.departmentList;
    this.positionList = result.positionList;
    this.contractTypeList = result.contractTypeList;
    this.employeeList = result.employeeList;
    this.initForm();
  });
}
displayFn = (id: string): string => {
  const emp = this.employeeList.find(e => e.id == id);
  return emp ? emp.form_name : '';
};

  ngOnDestroy(): void {
    this.valueChangesSub?.unsubscribe();
  }
  private initForm(): void {
    this.filterForm = this.fb.group({
      // Column filters
      contractCode: [''],
      employeeId: [''],
      departmentId: [''],
      signedDate: [''],
      endDate: [''],
      status: ['2'],
      signedDateFrom :[''],
      signedDateTo : [''],
      endDateFrom: [''],
      endDateTo : ['']
      
    });

    this.valueChangesSub = this.filterForm.valueChanges
      .pipe(debounceTime(300))
      .subscribe(() => this.applyFilters());
  }
  applyFilters(): void {
    const formValues = this.filterForm.value;
    console.log('Current form values:', formValues);
    // Tự động chuyển "" thành null cho columnFilters
    const columnKeys = ['contractCode', 'employeeId', 'departmentId', 'signedDate', 'endDate', 'status'];
    const columnFilters: { [key: string]: string | null } = {};
    columnKeys.forEach(key => {
      columnFilters[key] = formValues[key] || null;
    });

    const dateFilters: { [key: string]: DateRange } = {
      signedDate: {
        from: formValues.signedDateFrom || undefined,
        to: formValues.signedDateTo || undefined
      },
      endDate: {
        from: formValues.endDateFrom || undefined,
        to: formValues.endDateTo || undefined
      },
    };

    this.filterChange.emit({
      columnFilters,
      dateFilters
    });
  }

  resetFilters(): void {
  this.filterForm.reset({
    departmentId: '',
    contractCode: '',
    signedDate: '',
    endDate: '',
    status: '2', 
  });
  this.applyFilters();
}

}
