import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClockIcon } from './clock-icon';

describe('ClockIcon', () => {
  let component: ClockIcon;
  let fixture: ComponentFixture<ClockIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClockIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(ClockIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
