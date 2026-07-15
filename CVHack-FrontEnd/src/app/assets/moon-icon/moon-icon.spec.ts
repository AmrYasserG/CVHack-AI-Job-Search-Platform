import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoonIcon } from './moon-icon';

describe('MoonIcon', () => {
  let component: MoonIcon;
  let fixture: ComponentFixture<MoonIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MoonIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(MoonIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
