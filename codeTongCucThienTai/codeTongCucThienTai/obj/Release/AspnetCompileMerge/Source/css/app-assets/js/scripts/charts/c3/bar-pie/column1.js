/*=========================================================================================
    File Name: column.js
    Description: c3 column chart
    ----------------------------------------------------------------------------------------
    Item Name: Modern Admin - Clean Bootstrap 4 Dashboard HTML Template
    Version: 1.0
    Author: PIXINVENT
    Author URL: http://www.themeforest.net/user/pixinvent
==========================================================================================*/

// Column chart
// ------------------------------
$(window).on("load", function(){

    

    columnColors = ['#c0392b', '#c0392b', '#c0392b', '#c0392b', '#c0392b', '#c0392b', '#c0392b', '#c0392b', '#901c10', '#c0392b', '#c0392b', '#c0392b'];

    function setColumnBarColors(colors, chartContainer) {

        $('#' + chartContainer + ' .c3-chart-bars .c3-shape').each(function(index) {
          this.style.cssText += 'fill: ' + colors[index] + ' !important; stroke: ' + colors[index] + '; !important';
        });

        $('#' + chartContainer + ' .c3-chart-texts .c3-text').each(function(index) {
          this.style.cssText += 'fill: ' + colors[index] + ' !important;';
        });
      }

      

    // Callback that creates and populates a data table, instantiates the column chart, passes in the data and draws it.
    var columnChart = c3.generate({
        bindto: '#column-chart',
        size: {height:330},
        color: {
            pattern: ['#c0392b']
        },

        // Create the data table.
        data: {
            columns: [
                ['Tổng chi', -30, -200, -100, -100, -100, -100, -100, -100, -300, -100, -100, -100]
            ],
            type: 'bar'
        },

        
     
        bar: {
            width: {
                ratio: 0.7 // this makes bar width 50% of length between ticks
            }
            // or
            //width: 100 // this makes bar width 100px
        },
        axis: {
            x: {
                type: 'category',
                categories: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12']
                },
                
            y: {
                show: true,
                inverted: true,
                label: {
                    text: 'Số tiền',
                    position: 'outer-middle'
                  },
                  min: -400
            }
         

        }
    });

    setColumnBarColors(columnColors, 'column-chart');

      // Color turns to original when window is resized
      // To handle that
      $(window).resize(function() {
        setColumnBarColors(columnColors, 'column-chart');
      });
  
});