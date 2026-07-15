import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobCardsContainer } from './job-cards-container';

describe('JobCardsContainer', () => {
  let component: JobCardsContainer;
  let fixture: ComponentFixture<JobCardsContainer>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobCardsContainer],
    }).compileComponents();

    fixture = TestBed.createComponent(JobCardsContainer);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
