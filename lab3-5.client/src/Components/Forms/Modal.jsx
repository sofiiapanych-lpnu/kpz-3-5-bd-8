import { createPortal } from 'react-dom'
import { useRef, useEffect } from 'react'

export default function Modal({ children, open, onClick }) {
  const dialog = useRef()

  useEffect(() => {
      if (open) {
        dialog.current.showModal()
      } else {
        dialog.current.close()
      }
  }, [open])

  return createPortal( //dom
      <dialog ref={dialog}>
          {children}
      </dialog>,
      document.getElementById('modal')
  )
}