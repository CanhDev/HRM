import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { debounceTime, shareReplay } from 'rxjs';
import { ApiResponseBasic, PagedResult, SearchRequest, DateRange } from 'src/app/core/models/base';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';

@Component({
  selector: 'app-employee-filter',
  templateUrl: './employee-filter.component.html',
  styleUrls: ['./employee-filter.component.css']
})
export class EmployeeFilterComponent implements OnInit {
  @Output() filterChange = new EventEmitter<{
    columnFilters: { [key: string]: string | null },
    dateFilters: { [key: string]: DateRange }
  }>();

  filterForm!: FormGroup;
  private valueChangesSub: any;

  constructor(private fb: FormBuilder,private lookupService: LookupService) { }

    departments$ = this.lookupService.getDepartments().pipe(shareReplay(1));
    positions$ = this.lookupService.getPositions().pipe(shareReplay(1));
    statusList$ = this.lookupService.getSysListOptions("Employee", "status").pipe(shareReplay(1));

  ngOnInit(): void {
    this.initForm();
  }

  ngOnDestroy(): void {
    this.valueChangesSub?.unsubscribe();
  }

  private initForm(): void {
    this.filterForm = this.fb.group({
      // Column filters
      employeeCode: [''],
      gender: [''],
      departmentId: [''],
      positionId: [''],
      status: ['1'],
      // Date filters
      dobFrom: [''],
      dobTo: [''],
      joinDateFrom: [''],
      joinDateTo: [''],
    });

    this.valueChangesSub = this.filterForm.valueChanges
      .pipe(debounceTime(300))
      .subscribe(() => this.applyFilters());
  }

  applyFilters(): void {
    const formValues = this.filterForm.value;

    // Tự động chuyển "" thành null cho columnFilters
    const columnKeys = ['employeeCode', 'gender', 'positionId', 'departmentId', 'status'];
    const columnFilters: { [key: string]: string | null } = {};
    columnKeys.forEach(key => {
      columnFilters[key] = formValues[key] || null;
    });

    const dateFilters: { [key: string]: DateRange } = {
      dob: {
        from: formValues.dobFrom || undefined,
        to: formValues.dobTo || undefined
      },
      joinDate: {
        from: formValues.joinDateFrom || undefined,
        to: formValues.joinDateTo || undefined
      },
    };

    this.filterChange.emit({ columnFilters, dateFilters });
  }

  resetFilters(): void {
    this.filterForm.reset();
    this.applyFilters();
  }
}
