import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';


import { Department } from '../../entity';

@Component({
  selector: 'app-department-modal',
  templateUrl: './department-modal.component.html'
})
export class DepartmentModalComponent implements OnInit {
  departmentForm!: FormGroup;
  isEdit: boolean = false;
  statusList: any[];
  status: string = '';
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<DepartmentModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { department: Department, isEdit: boolean, statusList : any[] }
  ) {}

  ngOnInit(): void {
    this.isEdit = this.data.isEdit;
    this.statusList = this.data.statusList;
    this.departmentForm = this.fb.group({
      departmentCode: [this.data.department?.departmentCode || '', Validators.required],
      departmentName: [this.data.department?.departmentName || '', Validators.required],
      status: [this.data.department?.status || '1', Validators.required]
    });
  }

  save(): void {
    if (this.departmentForm.valid) {
      const result: Department = {
        ...this.data.department,
        ...this.departmentForm.value,
      };
      this.dialogRef.close(result);
    }
  }

  close(): void {
    this.dialogRef.close();
  }
}
