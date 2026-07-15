import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketIcon } from './ticket-icon';

describe('TicketIcon', () => {
  let component: TicketIcon;
  let fixture: ComponentFixture<TicketIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(TicketIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
