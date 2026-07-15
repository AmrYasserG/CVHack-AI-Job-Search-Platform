import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AiStarsIcon } from './ai-stars-icon';

describe('AiStarsIcon', () => {
  let component: AiStarsIcon;
  let fixture: ComponentFixture<AiStarsIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AiStarsIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(AiStarsIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
