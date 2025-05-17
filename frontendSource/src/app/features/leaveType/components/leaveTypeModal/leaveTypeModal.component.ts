import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { LeaveType } from '../../entity';

@Component({
  selector: 'app-leaveTypeModal',
  templateUrl: './leaveTypeModal.component.html',
  styleUrls: ['./leaveTypeModal.component.css']
})
export class LeaveTypeModalComponent implements OnInit {

  leaveTypeForm!: FormGroup;
    isEdit: boolean = false;
    statusList: any[];
    status: string = '';
    constructor(
      private fb: FormBuilder,
      private dialogRef: MatDialogRef<LeaveTypeModalComponent>,
      @Inject(MAT_DIALOG_DATA) public data: { LeaveType: LeaveType, isEdit: boolean, statusList : any[] }
    ) {}
  
    ngOnInit(): void {
      this.isEdit = this.data.isEdit;
      this.statusList = this.data.statusList;
      this.leaveTypeForm = this.fb.group({
        leaveTypeCode: [this.data.LeaveType?.leaveTypeCode || '', Validators.required],
        leaveTypeName: [this.data.LeaveType?.leaveTypeName || '', Validators.required],
        isPaid: [this.data.LeaveType?.isPaid || 0],
        maxDaysAllowed: [this.data.LeaveType?.maxDaysAllowed || 0],
        description:[this.data.LeaveType?.description || ''],
        status: [this.data.LeaveType?.status || '1', Validators.required]
      });
    }
  
    save(): void {
      if (this.leaveTypeForm.valid) {
        const result: LeaveType = {
          ...this.data.LeaveType,
          ...this.leaveTypeForm.value,
        };
        this.dialogRef.close(result);
      }
    }
  
    close(): void {
      this.dialogRef.close();
    }

}