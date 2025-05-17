import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PositionFilterComponent } from './components/position-filter/position-filter.component';
import { PositionsListComponent } from './components/positions-list/positions-list.component';
import { PositionsModalComponent } from './components/positions-modal/positions-modal.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { HttpClientModule } from '@angular/common/http';
import { PositionServiceService } from './positionService.service';

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
  declarations: [PositionFilterComponent, PositionsModalComponent, PositionsListComponent],
  providers:[PositionServiceService]
})
export class PositionsModule { }
