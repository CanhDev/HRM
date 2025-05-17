import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { EmployeeModule } from './features/employeeDoc/employeeDoc.module';
import { DocumentModule } from './features/document/document.module';
import { DepartmentsModule } from './features/departments/departments.module';
import { PositionsModule } from './features/positions/positions.module';
import { ContractTypeModule } from './features/contractType/contractType.module';
import { ContractsModule } from './features/contracts/contracts.module';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { SharedModule } from './shared/shared.module';
import { ToastrModule } from 'ngx-toastr';
// PrimeNG imports
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { CalendarModule } from 'primeng/calendar';
import { DialogModule } from 'primeng/dialog';
import { CardModule } from 'primeng/card';
import { InputNumberModule } from 'primeng/inputnumber';
import { TabViewModule } from 'primeng/tabview';
import { MessageModule } from 'primeng/message';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api'
import { LeaveTypeModule } from './features/leaveType/leaveType.module';
import { LeaveModule } from './features/leave/leave.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    EmployeeModule,
    DocumentModule,
    DepartmentsModule,
    PositionsModule,
    ContractTypeModule,
    ContractsModule,
    LeaveTypeModule,
    LeaveModule,
    //
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    CoreModule,
    SharedModule,
    StoreModule.forRoot({}),
    EffectsModule.forRoot([]),
    ToastrModule.forRoot({
      timeOut: 2000,
      preventDuplicates: true,
    }),
    // PrimeNG modules
    InputTextModule,
    DropdownModule,
    ButtonModule,
    TableModule,
    CalendarModule,
    DialogModule,
    CardModule,
    InputNumberModule,
    TabViewModule,
    MessageModule,
    ToastModule
  ],
  providers: [MessageService],
  bootstrap: [AppComponent]
})
export class AppModule { }
