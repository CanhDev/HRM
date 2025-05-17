import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Position } from '../../entity';

@Component({
  selector: 'app-positions-modal',
  templateUrl: './positions-modal.component.html',
  styleUrls: ['./positions-modal.component.css']
})
export class PositionsModalComponent implements OnInit {

  positionForm!: FormGroup;
    isEdit: boolean = false;
    statusList: any[];
    status: string = '';
    constructor(
      private fb: FormBuilder,
      private dialogRef: MatDialogRef<PositionsModalComponent>,
      @Inject(MAT_DIALOG_DATA) public data: { position: Position, isEdit: boolean, statusList : any[] }
    ) {}
  
    ngOnInit(): void {
      this.isEdit = this.data.isEdit;
      this.statusList = this.data.statusList;
      this.positionForm = this.fb.group({
        positionCode: [this.data.position?.positionCode || '', Validators.required],
        positionName: [this.data.position?.positionName || '', Validators.required],
        description :[this.data.position?.description || ''],
        status: [this.data.position?.status || '1', Validators.required]
      });
    }
  
    save(): void {
      if (this.positionForm.valid) {
        const result: Position = {
          ...this.data.position,
          ...this.positionForm.value,
        };
        this.dialogRef.close(result);
      }
    }
  
    close(): void {
      this.dialogRef.close();
    }

}
