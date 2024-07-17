"use client";
import * as React from "react"
import * as ScrollAreaPrimitive from "@radix-ui/react-scroll-area"

import { cn } from "@/lib/utils"
import { useEffect, useRef } from "react"
type ScrollAreaProps = React.ComponentPropsWithoutRef<typeof ScrollAreaPrimitive.Root> & {
  onScrollEnd?: () => void
}
const ScrollArea = React.forwardRef<
  React.ElementRef<typeof ScrollAreaPrimitive.Root>,
  ScrollAreaProps
>(({ className, onScrollEnd, children, ...props }, ref) => {
  const listInnerRef = useRef<HTMLDivElement>(null);
  useEffect(() => {
    let scrollEventListener: null | (() => void) = null;
  
    if (listInnerRef.current) {
      let scrollEventListener = () => {
        if (listInnerRef.current) {
          const { scrollTop, scrollHeight, clientHeight } = listInnerRef.current;
          if (scrollTop + clientHeight >= scrollHeight) {
            onScrollEnd?.();
          }
        }
      };
      listInnerRef.current.addEventListener('scroll', scrollEventListener);
    }
  
    return () => {
      if (listInnerRef.current && scrollEventListener) {
        listInnerRef.current.removeEventListener('scroll', scrollEventListener);
      }
    };
  }, [listInnerRef, onScrollEnd]);
  return (
    <ScrollAreaPrimitive.Root
      ref={ref}
      className={cn("relative overflow-hidden", className)}
      {...props}
    >
      <ScrollAreaPrimitive.Viewport ref={listInnerRef} className="h-full w-full rounded-[inherit]">
        {children}
      </ScrollAreaPrimitive.Viewport>
      <ScrollBar />
      <ScrollAreaPrimitive.Corner />
    </ScrollAreaPrimitive.Root>
  )
})
ScrollArea.displayName = ScrollAreaPrimitive.Root.displayName
const ScrollBar = React.forwardRef<
  React.ElementRef<typeof ScrollAreaPrimitive.ScrollAreaScrollbar>,
  React.ComponentPropsWithoutRef<typeof ScrollAreaPrimitive.ScrollAreaScrollbar>
>(({ className, orientation = "vertical", ...props }, ref) => {
  return (
    <ScrollAreaPrimitive.ScrollAreaScrollbar
      ref={ref}
      orientation={orientation}
      className={cn(
        "flex touch-none select-none transition-colors",
        orientation === "vertical" &&
          "h-full w-2.5 border-l border-l-transparent p-[1px]",
        orientation === "horizontal" &&
          "h-2.5 flex-col border-t border-t-transparent p-[1px]",
        className
      )}
      {...props}
    >
      <ScrollAreaPrimitive.ScrollAreaThumb className="relative flex-1 rounded-full bg-border" />
    </ScrollAreaPrimitive.ScrollAreaScrollbar>
  )
})
ScrollBar.displayName = ScrollAreaPrimitive.ScrollAreaScrollbar.displayName

export { ScrollArea, ScrollBar }
