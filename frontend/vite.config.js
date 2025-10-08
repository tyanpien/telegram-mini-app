import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    allowedHosts: ['7572e099167134.lhr.life'],
    host: true,
    port: 3000
  }
})
