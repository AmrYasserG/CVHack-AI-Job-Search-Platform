import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MagnifyingGlassIcon } from './magnifying-glass-icon';

describe('MagnifyingGlassIcon', () => {
  let component: MagnifyingGlassIcon;
  let fixture: ComponentFixture<MagnifyingGlassIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MagnifyingGlassIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(MagnifyingGlassIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
