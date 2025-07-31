import ContactSection from './components/ContactSection'
import DemoVideo from './components/DemoVideo'
import Footer from './components/Footer'
import Header from './components/Header'
import Hero from './components/Hero'
import MeetingSection from './components/MeetingSection'
import PricingSection from './components/PricingSection'
import ProblemSection from './components/ProblemSection'
import ScreenshotsSection from './components/ScreenshotsSection'
import SolutionSection from './components/SolutionSection'
import TestimonialsSection from './components/TestimonialsSection'
import WaitlistSection from './components/WaitlistSection'

export default function Home() {
  return (
    <main className="min-h-screen">
      <Header />
      <Hero />
      <ProblemSection />
      <SolutionSection />
      <ScreenshotsSection />
      <DemoVideo />
      <MeetingSection />
      <PricingSection />
      <TestimonialsSection />
      <WaitlistSection />
      <ContactSection />
      <Footer />
    </main>
  )
}
