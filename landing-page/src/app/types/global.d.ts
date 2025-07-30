declare global {
  interface Window {
    gtag: (
      command: 'config' | 'event',
      targetId: string,
      config?: {
        page_title?: string
        page_location?: string
        event_category?: string
        event_label?: string
      }
    ) => void
  }
}

export { }
