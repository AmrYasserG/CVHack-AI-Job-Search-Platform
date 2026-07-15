import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardIcon } from './dashboard-icon';

describe('DashboardIcon', () => {
  let component: DashboardIcon;
  let fixture: ComponentFixture<DashboardIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(DashboardIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
