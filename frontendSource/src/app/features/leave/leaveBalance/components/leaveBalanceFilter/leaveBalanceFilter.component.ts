import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { debounceTime, forkJoin, shareReplay } from 'rxjs';
import { ApiResponseBasic, PagedResult, SearchRequest, DateRange } from 'src/app/core/models/base';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';

@Component({
  selector: 'app-leaveBalanceFilter',
  templateUrl: './leaveBalanceFilter.component.html',
  styleUrls: ['./leaveBalanceFilter.component.css']
})
export class LeaveBalanceFilterComponent implements OnInit {

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
    this.initForm();
    forkJoin({
        statusList: this.lookupService.getSysListOptions("system", "status"),
        employeeList: this.lookupService.getEmployees()
      }).subscribe(result => {
        this.statusList = result.statusList;
        this.employeeList = result.employeeList;
        this.initForm();
      });
  }

  displayFn = (id: string): string => {
  const emp = this.employeeList.find(e => e.id == id);
  return emp ? emp.forn_value : '';
};
  ngOnDestroy(): void {
    this.valueChangesSub?.unsubscribe();
  }
  private initForm(): void {
    this.filterForm = this.fb.group({
      // Column filters
      employeeId: [''],
      status: ['1'],
      
    });

    this.valueChangesSub = this.filterForm.valueChanges
      .pipe(debounceTime(300))
      .subscribe(() => this.applyFilters());
  }
  applyFilters(): void {
    const formValues = this.filterForm.value;
    //console.log('Current form values:', formValues);
    // Tự động chuyển "" thành null cho columnFilters
    const columnKeys = ['employeeId', 'status'];
    const columnFilters: { [key: string]: string | null } = {};
    columnKeys.forEach(key => {
      columnFilters[key] = formValues[key] || null;
    });

    // const dateFilters: { [key: string]: DateRange } = {
    //   dob: {
    //     from: formValues.dobFrom || undefined,
    //     to: formValues.dobTo || undefined
    //   },
    //   joinDate: {
    //     from: formValues.joinDateFrom || undefined,
    //     to: formValues.joinDateTo || undefined
    //   },
    // };

    this.filterChange.emit({
      columnFilters,
      dateFilters: {}
    });
  }

  resetFilters(): void {
  this.filterForm.reset({
    employeeId: '',
    status: '1', 
  });
  this.applyFilters();
}

}
