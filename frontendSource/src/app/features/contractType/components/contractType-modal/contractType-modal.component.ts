import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { ContractType } from '../../entity';

@Component({
  selector: 'app-contractType-modal',
  templateUrl: './contractType-modal.component.html',
  styleUrls: ['./contractType-modal.component.css']
})
export class ContractTypeModalComponent implements OnInit {

  contractTypeForm!: FormGroup;
    isEdit: boolean = false;
    statusList: any[];
    status: string = '';
    constructor(
      private fb: FormBuilder,
      private dialogRef: MatDialogRef<ContractTypeModalComponent>,
      @Inject(MAT_DIALOG_DATA) public data: { contractType: ContractType, isEdit: boolean, statusList : any[] }
    ) {}
  
    ngOnInit(): void {
      this.isEdit = this.data.isEdit;
      this.statusList = this.data.statusList;
      this.contractTypeForm = this.fb.group({
        contractTypeCode: [this.data.contractType?.contractTypeCode || '', Validators.required],
        contractTypeName: [this.data.contractType?.contractTypeName || '', Validators.required],
        description:[this.data.contractType?.description || ''],
        status: [this.data.contractType?.status || '1', Validators.required]
      });
    }
  
    save(): void {
      if (this.contractTypeForm.valid) {
        const result: ContractType = {
          ...this.data.contractType,
          ...this.contractTypeForm.value,
        };
        this.dialogRef.close(result);
      }
    }
  
    close(): void {
      this.dialogRef.close();
    }

}
