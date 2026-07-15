import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SunIcon } from './sun-icon';

describe('SunIcon', () => {
  let component: SunIcon;
  let fixture: ComponentFixture<SunIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SunIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(SunIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
