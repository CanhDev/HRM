import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { EmployeeLeaveBalance } from '../../entity';
import { forkJoin, map, Observable, shareReplay, startWith } from 'rxjs';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';

@Component({
  selector: 'app-leaveBalanceModal',
  templateUrl: './leaveBalanceModal.component.html',
  styleUrls: ['./leaveBalanceModal.component.css']
})
export class LeaveBalanceModalComponent implements OnInit {

   employeeList$ = this.lookupService.getEmployees().pipe(shareReplay(1));;
    employeeList: any[] = [];
  filteredEmployees: Observable<any[]>;
  employeeName = '';

  

  EmployeeLeaveBalanceForm!: FormGroup;
    isEdit: boolean = false;
    statusList: any[];
    status: string = '';
    constructor(
      private lookupService : LookupService,
      private fb: FormBuilder,
      private dialogRef: MatDialogRef<LeaveBalanceModalComponent>,
      @Inject(MAT_DIALOG_DATA) public data: { EmployeeLeaveBalance: EmployeeLeaveBalance, isEdit: boolean, statusList : any[] }
    ) {}
  
    ngOnInit(): void {
      this.loadDropdownData();
      this.isEdit = this.data.isEdit;
      this.statusList = this.data.statusList;
      this.EmployeeLeaveBalanceForm = this.fb.group({
        employeeId: [this.data.EmployeeLeaveBalance?.employeeId || '', Validators.required],
        totalDays: [this.data.EmployeeLeaveBalance?.totalDays || '', Validators.required],
        usedDays: [this.data.EmployeeLeaveBalance?.usedDays || 0, Validators.required],
        remainingDays : [this.data.EmployeeLeaveBalance?.remainingDays || 0, Validators.required],
        usedDaysMonth : [this.data.EmployeeLeaveBalance?.usedDaysMonth || 0, Validators.required],
        remainingDaysMonth : [this.data.EmployeeLeaveBalance?.remainingDaysMonth || 0, Validators.required],
        maxDayMonth : [this.data.EmployeeLeaveBalance?.maxDayMonth || 0, Validators.required],
        status: [this.data.EmployeeLeaveBalance?.status || '1', Validators.required]
      });
      this.employeeList$.subscribe(data =>{
         this.employeeList = data
         this.employeeName = this.getEmployeeName(this.data.EmployeeLeaveBalance.employeeId.toString());
      });
      this.initFormsWithData();
    }
    initFormsWithData(): void {
    
    
    if (this.isEdit) {
      this.loadData();
    }
  }
    loadData() : void{
        this.EmployeeLeaveBalanceForm = this.fb.group({
        employeeId: [this.data.EmployeeLeaveBalance?.employeeId || '', Validators.required],
        totalDays: [this.data.EmployeeLeaveBalance?.totalDays || '', Validators.required],
        usedDays: [this.data.EmployeeLeaveBalance?.usedDays || 0, Validators.required],
        remainingDays : [this.data.EmployeeLeaveBalance?.remainingDays || 0, Validators.required],
        usedDaysMonth : [this.data.EmployeeLeaveBalance?.usedDaysMonth || 0, Validators.required],
        remainingDaysMonth : [this.data.EmployeeLeaveBalance?.remainingDaysMonth || 0, Validators.required],
        maxDayMonth : [this.data.EmployeeLeaveBalance?.maxDayMonth || 0, Validators.required],
        status: [this.data.EmployeeLeaveBalance?.status || '1', Validators.required]
      });
    }
    loadDropdownData(): void {
        forkJoin({
          employeeList: this.lookupService.getEmployees()
        }).subscribe({
          next: (result) => {
            this.employeeList = result.employeeList;
            this.loadData();
          },
          error: (error) => {
            console.error('Lỗi khi tải dữ liệu dropdown:', error);
          }
        });
      }
      getEmployeeName(id: string | null): string {
        console.log(id, this.employeeList);
    if (!id) return '';
    const employee = this.employeeList.find(emp => emp.id == id);
    return employee ? employee.form_name : '';
  }
  setupAutoComplete(): void {
  
    const employeeIdControl = this.EmployeeLeaveBalanceForm.get('employeeId');
    this.employeeName = this.getEmployeeName(this.data.EmployeeLeaveBalance.employeeId.toString());
    console.log(this.employeeList);
    if (employeeIdControl) {
      this.filteredEmployees = employeeIdControl.valueChanges.pipe(
        startWith(''),
        map(value => {
          if (typeof value === 'string') {
            return this._filterEmployees(value);
          } else {
            // If it's already an ID, keep showing all employees
            return this.employeeList.slice();
          }
        })
      );
    }
  }
private _filterEmployees(value: string): any[] {
  const filterValue = value.toLowerCase();
  return this.employeeList.filter(emp => 
    emp.form_name.toLowerCase().includes(filterValue) || 
    (emp.form_value && emp.form_value.toString().toLowerCase().includes(filterValue))
  );
}
 

  // Phương thức hiển thị tên nhân viên trong trường nhập
 displayFn = (id: string | number | null): string => {
  if (id === null || id === undefined) return '';
  
  // Convert to string for comparison if needed
  const employeeId = id.toString();
  const emp = this.employeeList.find(e => e.id.toString() === employeeId);
  return emp ? emp.form_value : '';
};

  // Phương thức đảm bảo chỉ có thể chọn nhân viên hợp lệ
  validateSelection(): void {
  const employeeIdControl = this.EmployeeLeaveBalanceForm.get('employeeId');
  if (!employeeIdControl) return;
  
  const currentValue = employeeIdControl.value;
  
  // If it's a string (user typing), find matching employee
  if (typeof currentValue === 'string') {
    const matchingEmployee = this.employeeList.find(
      emp => emp.id.toLowerCase() === currentValue.toLowerCase()
    );
    
    if (!matchingEmployee) {
      employeeIdControl.setValue(null);
    } else {
      employeeIdControl.setValue(matchingEmployee.id);
    }
  } 
  // If it's not a string but also not a valid ID in our list, reset it
  else if (currentValue !== null) {
    const exists = this.employeeList.some(emp => emp.id === currentValue);
    if (!exists) {
      employeeIdControl.setValue(null);
    }
  }
}

    save(): void {
      if (this.EmployeeLeaveBalanceForm.valid) {
        const result: EmployeeLeaveBalance = {
          ...this.data.EmployeeLeaveBalance,
          ...this.EmployeeLeaveBalanceForm.value,
        };
        this.dialogRef.close(result);
      }
    }
  
    close(): void {
      this.dialogRef.close();
    }

}

