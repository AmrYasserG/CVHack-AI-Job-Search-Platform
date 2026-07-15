import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LockIcon } from './lock-icon';

describe('LockIcon', () => {
  let component: LockIcon;
  let fixture: ComponentFixture<LockIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LockIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(LockIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
