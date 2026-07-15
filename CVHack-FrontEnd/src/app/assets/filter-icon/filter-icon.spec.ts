import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterIcon } from './filter-icon';

describe('FilterIcon', () => {
  let component: FilterIcon;
  let fixture: ComponentFixture<FilterIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FilterIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(FilterIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
