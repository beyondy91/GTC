#
# This is a Shiny web application. You can run the application by clicking
# the 'Run App' button above.
#
# Find out more about building applications with Shiny here:
#
#    http://shiny.rstudio.com/
#

library(shiny)
library(jsonlite)
library(DT)
library(dplyr)
library(doParallel)

# Define UI for application that draws a histogram
ui <- fluidPage(
  tabsetPanel(
    tabPanel(title="API (internal)",
             DTOutput('api.table.internal')),
    tabPanel(title="API (open)",
             DTOutput('api.table.open'))
  )
)

# Define server logic required to draw a histogram
server <- function(input, output) {
  
  api.table <- function() {
    registerDoParallel(detectCores())
    dat <- fromJSON('resource/api.json')
    foreach(key = names(dat), .combine=rbind) %dopar% {
      data.frame(API=gsub('APIFunction-API$', '', key),
                 URL=dat[[key]]$url,
                 FunctionDescription=toString(dat[[key]]$description[['function']][[1]]),
                 RequestDescription=toString(toJSON(dat[[key]]$description$request)),
                 RequestDescription=toString(toJSON(dat[[key]]$description$response)))
    }
  }
  
  output$api.table.internal <- renderDT({
    api.table() %>%
      filter(grepl('^Internal', API, ignore.case=TRUE))
  })
  output$api.table.open <- renderDT({
    api.table() %>%
      filter(grepl('^Open', API, ignore.case=TRUE))
  })
}

# Run the application 
shinyApp(ui = ui, server = server)

