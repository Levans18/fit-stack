/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './index.html',
    './src/**/*.{js,ts,jsx,tsx}',
  ],
  theme: {
    extend: {
      animation: {
        'pulse-slow': 'pulse 8s ease-in-out infinite',
        'pulse-fast': 'pulse 4s ease-in-out infinite',
      },
      colors: {
        border: 'hsl(var(--border))',
        background: 'hsl(var(--background))',
        foreground: 'hsl(var(--foreground))',
        ring: 'hsl(var(--ring))',
        input: 'hsl(var(--input))',
    
        // 🔵 FitStack blues
        brand: {
          DEFAULT: '#2563eb',     // blue-600
          dark: '#1e40af',        // blue-800
          light: '#3b82f6',       // blue-500
          background: '#eff6ff',  // blue-50
        },
      },
    },
  },
  plugins: [],
}