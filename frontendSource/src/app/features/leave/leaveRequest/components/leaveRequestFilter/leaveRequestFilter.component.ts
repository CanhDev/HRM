import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { debounceTime, forkJoin, map, Observable, shareReplay, startWith } from 'rxjs';
import { ApiResponseBasic, PagedResult, SearchRequest, DateRange } from 'src/app/core/models/base';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';

@Component({
  selector: 'app-leaveRequestFilter',
  templateUrl: './leaveRequestFilter.component.html',
  styleUrls: ['./leaveRequestFilter.component.css']
})
export class LeaveRequestFilterComponent implements OnInit {

 @Output() filterChange = new EventEmitter<{
    columnFilters: { [key: string]: string | null },
    dateFilters: { [key: string]: DateRange }
  }>();
  filterForm!: FormGroup;
  private valueChangesSub: any;
  constructor(private fb: FormBuilder,private lookupService: LookupService) {}
  

  statusList: any[] = [];
  employeeList: any[] = [];


  ngOnInit() {
  forkJoin({
    statusList: this.lookupService.getSysDmtt("LEAVE"),
    employeeList: this.lookupService.getEmployees()
  }).subscribe(result => {
    this.statusList = result.statusList;
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
      employeeId: [''],
      voucher_code: [''],
      status: [''],
      createdAt : [''],
      createdAtFrom: [''],
      createdAtTo: [''],
    });

    this.valueChangesSub = this.filterForm.valueChanges
      .pipe(debounceTime(300))
      .subscribe(() => this.applyFilters());
  }
  applyFilters(): void {
    const formValues = this.filterForm.value;
    console.log('Current form values:', formValues);
    // Tự động chuyển "" thành null cho columnFilters
    const columnKeys = ['employeeId', 'createdAt', 'voucher_code', 'status'];
    const columnFilters: { [key: string]: string | null } = {};
    columnKeys.forEach(key => {
      columnFilters[key] = formValues[key] || null;
    });

    const dateFilters: { [key: string]: DateRange } = {
      createdAt: {
        from: formValues.createdAtFrom || undefined,
        to: formValues.createdAtTo || undefined
      },
    };

    this.filterChange.emit({
      columnFilters,
      dateFilters
    });
  }

  resetFilters(): void {
  this.filterForm.reset({
    voucher_code: '',
    employeeId: '',
    createdAt: '',
    status: '', 
    createdAtFrom: '',
    createdAtTo: '',
  });
  this.applyFilters();
}

}