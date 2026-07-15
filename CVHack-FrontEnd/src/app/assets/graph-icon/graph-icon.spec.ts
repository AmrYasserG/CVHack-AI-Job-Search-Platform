import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GraphIcon } from './graph-icon';

describe('GraphIcon', () => {
  let component: GraphIcon;
  let fixture: ComponentFixture<GraphIcon>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GraphIcon],
    }).compileComponents();

    fixture = TestBed.createComponent(GraphIcon);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
