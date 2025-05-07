/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { Http_optionsService } from './http_options.service';

describe('Service: Http_options', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [Http_optionsService]
    });
  });

  it('should ...', inject([Http_optionsService], (service: Http_optionsService) => {
    expect(service).toBeTruthy();
  }));
});
