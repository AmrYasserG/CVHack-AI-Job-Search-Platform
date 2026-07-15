import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeaturePills } from './feature-pills';

describe('FeaturePills', () => {
  let component: FeaturePills;
  let fixture: ComponentFixture<FeaturePills>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FeaturePills],
    }).compileComponents();

    fixture = TestBed.createComponent(FeaturePills);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
