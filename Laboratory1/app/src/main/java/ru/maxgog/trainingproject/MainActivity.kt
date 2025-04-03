package ru.maxgog.trainingproject

import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import android.text.Editable
import android.text.TextWatcher
import androidx.activity.ComponentActivity

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        val editText: EditText = findViewById(R.id.editText)
        val textView: TextView = findViewById(R.id.textView)
        val changeTextButton: Button = findViewById(R.id.editButton)
        val clearTextButton: Button = findViewById(R.id.cleanButton)

        changeTextButton.setOnClickListener {
            val inputText = editText.text.toString()
            if (inputText.isNotEmpty()) {
                textView.text = inputText
            } else {
                textView.text = "Please input text..."
            }
        }

        clearTextButton.setOnClickListener {
            editText.text.clear()
            textView.text = ""
        }

        editText.addTextChangedListener(object : TextWatcher {
            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {}

            override fun onTextChanged(s: CharSequence?, start: Int, before: Int, count: Int) {
                textView.text = s.toString()
            }

            override fun afterTextChanged(s: Editable?) {}
        })
    }
}
