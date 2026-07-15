import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmitTicketForm } from './submit-ticket-form';

describe('SubmitTicketForm', () => {
  let component: SubmitTicketForm;
  let fixture: ComponentFixture<SubmitTicketForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubmitTicketForm],
    }).compileComponents();

    fixture = TestBed.createComponent(SubmitTicketForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
