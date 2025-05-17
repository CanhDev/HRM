import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule } from '@angular/common/http';

import { ContractTypeService } from './contractType.service';
import { ContractTypeListComponent } from './components/contractType-list/contractType-list.component';
import { ContractTypeFilterComponent } from './components/contractType-filter/contractType-filter.component';
import { ContractTypeModalComponent } from './components/contractType-modal/contractType-modal.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    HttpClientModule
  ],
  declarations: [ContractTypeModalComponent, ContractTypeListComponent, ContractTypeFilterComponent],
  providers: [ContractTypeService]
})
export class ContractTypeModule { }
