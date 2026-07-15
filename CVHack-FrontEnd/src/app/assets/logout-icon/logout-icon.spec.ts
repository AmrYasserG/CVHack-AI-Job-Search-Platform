import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LogoutIcon } from './logout-icon';

describe('LogoutIcon', () => {
  let component: LogoutIcon;
  let fixture: ComponentFixture<LogoutIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LogoutIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(LogoutIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
