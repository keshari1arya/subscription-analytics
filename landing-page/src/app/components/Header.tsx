'use client'

import { motion } from 'framer-motion'
import { Menu, X } from 'lucide-react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { useState } from 'react'
import Button from './Button'

export default function Header() {
  const [isMenuOpen, setIsMenuOpen] = useState(false)
  const pathname = usePathname()

  const navigation = [
    { name: 'Features', href: '/#features' },
    { name: 'Demo', href: '/#screenshots' },
    { name: 'Compare', href: '/compare' },
    { name: 'Pricing', href: '/#pricing' },
    { name: 'About', href: '/about' },
  ]

  const scrollToSection = (href: string) => {
    const element = document.querySelector(href.replace('/', ''))
    if (element) {
      element.scrollIntoView({
        behavior: 'smooth',
        block: 'start'
      })
    }
    setIsMenuOpen(false)
  }

  const handleNavigation = (href: string) => {
    if (href.startsWith('/#') || href.startsWith('#')) {
      // Internal anchor - scroll to section
      if (pathname === '/') {
        scrollToSection(href)
      } else {
        // Navigate to home page first, then scroll
        window.location.href = href
      }
    } else if (href.startsWith('/')) {
      // External page - navigate
      window.location.href = href
    } else {
      // Internal anchor - scroll to section
      scrollToSection(href)
    }
    setIsMenuOpen(false)
  }

  return (
    <header className="fixed top-0 left-0 right-0 z-50 bg-white/80 backdrop-blur-md border-b border-gray-200">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          {/* Logo */}
          <motion.div
            initial={{ opacity: 0, x: -20 }}
            animate={{ opacity: 1, x: 0 }}
            className="flex items-center"
          >
            <Link href="/" className="flex-shrink-0">
              <h1 className="text-2xl font-bold text-blue-600 hover:text-blue-700 transition-colors cursor-pointer">
                SubscriptionAnalytics
              </h1>
            </Link>
          </motion.div>

          {/* Desktop Navigation */}
          <nav className="hidden lg:flex space-x-8">
            {navigation.map((item) => (
              <motion.button
                key={item.name}
                onClick={() => handleNavigation(item.href)}
                className="text-gray-700 hover:text-blue-600 px-3 py-2 text-sm font-medium transition-colors duration-200 cursor-pointer"
                whileHover={{ y: -2 }}
              >
                {item.name}
              </motion.button>
            ))}
          </nav>

          {/* Desktop CTA */}
          <div className="hidden md:flex items-center space-x-4">
            <Button variant="outline" size="sm" onClick={() => handleNavigation('/#waitlist')}>
              Join Waitlist
            </Button>
          </div>

          {/* Mobile menu button */}
          <div className="md:hidden">
            <button
              onClick={() => setIsMenuOpen(!isMenuOpen)}
              className="text-gray-700 hover:text-blue-600 p-2 cursor-pointer"
            >
              {isMenuOpen ? <X size={24} /> : <Menu size={24} />}
            </button>
          </div>
        </div>

        {/* Mobile Navigation */}
        {isMenuOpen && (
          <motion.div
            initial={{ opacity: 0, y: -20 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0, y: -20 }}
            className="md:hidden"
          >
            <div className="px-2 pt-2 pb-3 space-y-1 sm:px-3 bg-white border-t border-gray-200">
              {navigation.map((item) => (
                <button
                  key={item.name}
                  onClick={() => handleNavigation(item.href)}
                  className="text-gray-700 hover:text-blue-600 block px-3 py-2 text-base font-medium w-full text-left cursor-pointer"
                >
                  {item.name}
                </button>
              ))}
              <div className="pt-4">
                <Button variant="outline" size="sm" onClick={() => handleNavigation('/#waitlist')} className="w-full">
                  Join Waitlist
                </Button>
              </div>
            </div>
          </motion.div>
        )}
      </div>
    </header>
  )
}
