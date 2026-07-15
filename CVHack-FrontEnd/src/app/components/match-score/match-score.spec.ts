import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MatchScore } from './match-score';

describe('MatchScore', () => {
  let component: MatchScore;
  let fixture: ComponentFixture<MatchScore>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MatchScore],
    }).compileComponents();

    fixture = TestBed.createComponent(MatchScore);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
