package ru.maxgog.calculatorapp

import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import androidx.activity.ComponentActivity
import androidx.activity.enableEdgeToEdge

class MainActivity : ComponentActivity() {

    private lateinit var resultView: EditText
    private lateinit var historyView: TextView
    private var currentNumber: String = ""
    private var currentOperation: String? = null
    private var firstOperand: Double? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main)

        resultView = findViewById(R.id.resultView)
        historyView = findViewById(R.id.historyView)

        findViewById<Button>(R.id.btn1).setOnClickListener { appendNumber("1") }
        findViewById<Button>(R.id.btn2).setOnClickListener { appendNumber("2") }
        findViewById<Button>(R.id.btn3).setOnClickListener { appendNumber("3") }
        findViewById<Button>(R.id.btn4).setOnClickListener { appendNumber("4") }
        findViewById<Button>(R.id.btn5).setOnClickListener { appendNumber("5") }
        findViewById<Button>(R.id.btn6).setOnClickListener { appendNumber("6") }
        findViewById<Button>(R.id.btn7).setOnClickListener { appendNumber("7") }
        findViewById<Button>(R.id.btn8).setOnClickListener { appendNumber("8") }
        findViewById<Button>(R.id.btn9).setOnClickListener { appendNumber("9") }
        findViewById<Button>(R.id.btn0).setOnClickListener { appendNumber("0") }
        findViewById<Button>(R.id.btnPlus).setOnClickListener { setOperation("+") }
        findViewById<Button>(R.id.btnMinus).setOnClickListener { setOperation("-") }
        findViewById<Button>(R.id.btnMulti).setOnClickListener { setOperation("*") }
        findViewById<Button>(R.id.btnDivide).setOnClickListener { setOperation("/") }
        findViewById<Button>(R.id.btnResult).setOnClickListener { calculateResult() }
        findViewById<Button>(R.id.btnClear).setOnClickListener { clearAll() }

    }

    private fun appendNumber(number: String) {
        currentNumber += number
        resultView.setText(currentNumber)
    }

    private fun setOperation(operation: String) {
        if (currentNumber.isNotEmpty()) {
            firstOperand = currentNumber.toDouble()
            currentNumber = ""
            currentOperation = operation
        }
    }

    private fun calculateResult() {
        if (currentOperation != null && currentNumber.isNotEmpty()) {
            val secondOperand = currentNumber.toDouble()
            if (currentOperation == "/" && secondOperand == 0.0) {
                resultView.setText("Ошибка: деление на ноль")
                return
            }
            val result = when (currentOperation) {
                "+" -> firstOperand!! + secondOperand
                "-" -> firstOperand!! - secondOperand
                "*" -> firstOperand!! * secondOperand
                "/" -> if (secondOperand != 0.0) firstOperand!! / secondOperand else "Error"
                else -> "Error"
            }
            resultView.setText(result.toString())

            val historyText = "$firstOperand $currentOperation $secondOperand = $result\n"
            historyView.append(historyText)


            firstOperand = null
            currentOperation = null
            currentNumber = ""
        }
    }

    private fun clearAll() {
        currentNumber = ""
        firstOperand = null
        currentOperation = null
        resultView.setText("")
    }


}
