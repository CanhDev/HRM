/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ContractTypeService } from './contractType.service';

describe('Service: ContractType', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ContractTypeService]
    });
  });

  it('should ...', inject([ContractTypeService], (service: ContractTypeService) => {
    expect(service).toBeTruthy();
  }));
});
