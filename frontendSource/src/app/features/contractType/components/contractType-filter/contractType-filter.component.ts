import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { debounceTime, shareReplay } from 'rxjs';
import { ApiResponseBasic, PagedResult, SearchRequest, DateRange } from 'src/app/core/models/base';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';

@Component({
  selector: 'app-contractType-filter',
  templateUrl: './contractType-filter.component.html',
  styleUrls: ['./contractType-filter.component.css']
})
export class ContractTypeFilterComponent implements OnInit {

  @Output() filterChange = new EventEmitter<{
    columnFilters: { [key: string]: string | null },
    dateFilters: { [key: string]: DateRange }
  }>();
  filterForm!: FormGroup;
  private valueChangesSub: any;
  constructor(private fb: FormBuilder,private lookupService: LookupService) {}
  statusList$ = this.lookupService.getSysListOptions("system", "status");
  statusList: any[] = [];


  ngOnInit() {
    this.statusList$.subscribe(data => {
      this.statusList = data;
    });
    this.initForm();
  }
  ngOnDestroy(): void {
    this.valueChangesSub?.unsubscribe();
  }
  private initForm(): void {
    this.filterForm = this.fb.group({
      // Column filters
      contractTypeCode: [''],
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
    const columnKeys = ['contractTypeCode', 'status'];
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
    contractTypeCode: '',
    status: '1', 
  });
  this.applyFilters();
}

}
