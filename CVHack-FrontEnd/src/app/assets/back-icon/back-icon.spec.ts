import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackIcon } from './back-icon';

describe('BackIcon', () => {
  let component: BackIcon;
  let fixture: ComponentFixture<BackIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BackIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(BackIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
