import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PassStrengthIcon } from './pass-strength-icon';

describe('PassStrengthIcon', () => {
  let component: PassStrengthIcon;
  let fixture: ComponentFixture<PassStrengthIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PassStrengthIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(PassStrengthIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
