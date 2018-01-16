import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SampleValidationComponent } from './sample-validation.component';

describe('SampleValidationComponent', () => {
  let component: SampleValidationComponent;
  let fixture: ComponentFixture<SampleValidationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SampleValidationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SampleValidationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
