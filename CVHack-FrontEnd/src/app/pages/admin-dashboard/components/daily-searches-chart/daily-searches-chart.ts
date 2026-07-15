import { Component, Input, AfterViewInit, ElementRef, ViewChild } from '@angular/core';

export interface DailyPoint { day: string; value: number; }

@Component({
  selector: 'app-daily-searches-chart',
  imports: [],
  templateUrl: './daily-searches-chart.html',
  styleUrl: './daily-searches-chart.css',
})
export class DailySearchesChart implements AfterViewInit {
  @Input() data: DailyPoint[] = [];
  @ViewChild('canvas') canvasRef!: ElementRef<HTMLCanvasElement>;

  ngAfterViewInit() {
    this.draw();
  }

  draw() {
    const canvas = this.canvasRef.nativeElement;
    const ctx = canvas.getContext('2d')!;
    const W = canvas.offsetWidth;
    const H = canvas.offsetHeight;
    canvas.width = W;
    canvas.height = H;

    const pad = { top: 20, right: 20, bottom: 40, left: 20 };
    const chartW = W - pad.left - pad.right;
    const chartH = H - pad.top - pad.bottom;

    const vals = this.data.map(d => d.value);
    const min = Math.min(...vals) * 0.9;
    const max = Math.max(...vals) * 1.05;

    const xStep = chartW / (this.data.length - 1);
    const yScale = (v: number) => pad.top + chartH - ((v - min) / (max - min)) * chartH;

    const pts = this.data.map((d, i) => ({ x: pad.left + i * xStep, y: yScale(d.value) }));

    // Fill gradient
    const grad = ctx.createLinearGradient(0, pad.top, 0, pad.top + chartH);
    grad.addColorStop(0, 'rgba(184,115,51,.18)');
    grad.addColorStop(1, 'rgba(184,115,51,.0)');

    ctx.beginPath();
    ctx.moveTo(pts[0].x, pts[0].y);
    for (let i = 1; i < pts.length; i++) {
      const cp = (pts[i].x + pts[i-1].x) / 2;
      ctx.bezierCurveTo(cp, pts[i-1].y, cp, pts[i].y, pts[i].x, pts[i].y);
    }
    ctx.lineTo(pts[pts.length-1].x, pad.top + chartH);
    ctx.lineTo(pts[0].x, pad.top + chartH);
    ctx.closePath();
    ctx.fillStyle = grad;
    ctx.fill();

    // Line
    ctx.beginPath();
    ctx.moveTo(pts[0].x, pts[0].y);
    for (let i = 1; i < pts.length; i++) {
      const cp = (pts[i].x + pts[i-1].x) / 2;
      ctx.bezierCurveTo(cp, pts[i-1].y, cp, pts[i].y, pts[i].x, pts[i].y);
    }
    ctx.strokeStyle = '#B87333';
    ctx.lineWidth = 2.5;
    ctx.stroke();

    // Dots
    pts.forEach(p => {
      ctx.beginPath();
      ctx.arc(p.x, p.y, 4, 0, Math.PI * 2);
      ctx.fillStyle = '#fff';
      ctx.fill();
      ctx.strokeStyle = '#B87333';
      ctx.lineWidth = 2;
      ctx.stroke();
    });

    // Day labels
    ctx.fillStyle = '#B87333';
    ctx.font = `500 12px IBM Plex Sans, sans-serif`;
    ctx.textAlign = 'center';
    this.data.forEach((d, i) => {
      ctx.fillText(d.day, pad.left + i * xStep, H - 10);
    });
  }
}
