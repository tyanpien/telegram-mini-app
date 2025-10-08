import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    allowedHosts: ['fb0fcc3e2cd4aa.lhr.life'],
    host: true,
    port: 3000
  }
})
