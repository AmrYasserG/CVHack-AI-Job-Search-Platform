import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyIcon } from './company-icon';

describe('CompanyIcon', () => {
  let component: CompanyIcon;
  let fixture: ComponentFixture<CompanyIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CompanyIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(CompanyIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
